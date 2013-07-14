using System;
using System.Windows;

namespace LayerCake.UI.Services
{
    internal sealed class DataTemplateContainer
    {
        private readonly Lazy<DataTemplate> dataTemplate;

        public DataTemplateContainer(Type type, Uri uri)
        {
            this.dataTemplate = new Lazy<DataTemplate>(() => FindDataTemplate(type, uri));
        }

        private DataTemplate FindDataTemplate(Type type, Uri uri)
        {
            var resourceDictionary = new ResourceDictionary
            {
                Source = uri
            };

            return (DataTemplate)resourceDictionary[new DataTemplateKey(type)];
        }

        public DataTemplate DataTemplate 
        {
            get { return dataTemplate.Value; }
        }
    }
}
