// Expose internal types to the test assembly
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("OnlineCoursesPlatform.Tests.Integration")]

// Make Program accessible for WebApplicationFactory
public partial class Program { }
