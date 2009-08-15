using System;
using System.Collections.Generic;
using NHaml.Exceptions;
using NHaml.TemplateResolution;
using NHaml.Utils;

namespace NHaml
{
    public class CompiledTemplate
    {
        private readonly TemplateEngine _templateEngine;

        private readonly IList<IViewSource> _layoutViewSources;

        private readonly Type _templateBaseType;

        private TemplateFactory _templateFactory;

        private readonly object _sync = new object();
        private IList<Func<bool>> viewSourceModifiedChecks;

        internal CompiledTemplate( TemplateEngine templateEngine, IList<IViewSource> layoutViewSources, Type templateBaseType )
        {
            _templateEngine = templateEngine;
            _layoutViewSources = layoutViewSources;
            _templateBaseType = templateBaseType;

            Compile();
        }

        public Template CreateInstance()
        {
            return _templateFactory.CreateTemplate();
        }

        public void Recompile()
        {
            lock (_sync)
            {
                foreach (var inputFile in viewSourceModifiedChecks)
                {
                    if (inputFile())
                    {
                        Compile();
                        break;
                    }
                }
            }
        }

        private void Compile()
        {
            var className = Utility.MakeClassName( ListExtensions.Last(_layoutViewSources).Path );
            var templateClassBuilder = _templateEngine.Options.TemplateCompiler.CreateTemplateClassBuilder(className, _templateBaseType );

            var templateParser = new TemplateParser(_templateEngine, templateClassBuilder, _layoutViewSources);

            templateParser.Parse();
            viewSourceModifiedChecks = templateParser.ViewSourceModifiedChecks;

            if( _templateBaseType.IsGenericTypeDefinition )
            {
                string model;
                Type modelType = null;

                if( templateParser.Meta.TryGetValue( "model", out model ) )
                {
                    foreach( var assembly in AppDomain.CurrentDomain.GetAssemblies() )
                    {
                        modelType = assembly.GetType( model, false, true );

                        if( modelType != null )
                            break;
                    }

                    if( modelType == null )
                    {
                        var message = string.Format("The given model type '{0}' was not found.", model );
                        throw new TemplateCompilationException( message );
                    }
                }
                else
                {
                    modelType = typeof (object);
                }

                templateClassBuilder.BaseType = _templateBaseType.MakeGenericType( modelType );

                _templateEngine.Options.AddReference( modelType.Assembly );
            }

            _templateFactory = _templateEngine.Options.TemplateCompiler.Compile( templateParser );

         
        }
    }
}