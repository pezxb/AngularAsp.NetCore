using Swashbuckle.AspNetCore.Swagger;

namespace BackEnd
{
    internal class VersionControl : Info
    {
        public string Title { get; set; }
        public string Version { get; set; }
    }
}