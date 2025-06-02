using Microsoft.Extensions.Hosting;
using Moq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Tests
{
    public class ApplicationExtentionTests
    {
        [Fact]
        public void ConfigureSerilog_DoesNotThrow()
        {
            //var hostBuilderMock = new Mock<IHostBuilder>();
            //hostBuilderMock.Setup(h => h.UseSerilog(It.IsAny<Action<HostBuilderContext, IServiceProvider, LoggerConfiguration>>()))
            //    .Returns(hostBuilderMock.Object);

            //// Should not throw
            //hostBuilderMock.Object.ConfigureSerilog();
        }
    }
}
