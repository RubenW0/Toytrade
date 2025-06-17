using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

public class FakeHostEnvironment : IHostEnvironment
{
    public string EnvironmentName { get; set; } = "Development";
    public string ApplicationName { get; set; } = "ToyApp";
    public string ContentRootPath { get; set; } = "C:\\FakeRoot";
    public IFileProvider ContentRootFileProvider { get; set; } = null!;
}
