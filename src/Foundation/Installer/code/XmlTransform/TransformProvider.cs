namespace Sitecore.Foundation.Installer.XmlTransform
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class TransformProvider : ITransformsProvider
    {
        private const string transformsPath = "~/temp/transforms";
        private readonly IFilePathResolver filePathResolver;

        public TransformProvider() : this(new FilePathResolver())
        {
        }

        public TransformProvider(IFilePathResolver path)
        {
            this.filePathResolver = path;
        }

        public List<string> GetTransformsByLayer(string layerName)
        {
            var transforms = new List<string>();

            var transformsFolderPath = this.filePathResolver.MapPath(transformsPath);
            if (string.IsNullOrEmpty(transformsFolderPath))
            {
                return transforms;
            }

            var transformsFolder = new DirectoryInfo(transformsFolderPath);

            var layerFolder = transformsFolder.GetDirectories(layerName).FirstOrDefault();
            if (layerFolder != null && layerFolder.Exists)
            {
                transforms.AddRange(layerFolder.GetFiles("*.xdt", SearchOption.AllDirectories).Select(transformFile => transformFile.FullName));
            }

            return transforms;
        }
    }
}