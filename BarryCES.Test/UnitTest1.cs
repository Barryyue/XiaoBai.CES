using System.Collections.Generic;
using AutoMapper;
using BarryCES.Infrastructure;
using BarryCES.Infrastructure.Extentions;
using BarryCES.Interfaces;
using BarryCES.Models;
using BarryCES.Services;
using Mehdime.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;

namespace BarryCES.Test
{
    [TestClass]
    public class ProjectTest
    {
        [TestMethod]
        public void AddProject()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new ExecutionContextScopeLifestyle();

            // Register your types, for instance:
            RegisterService(container);

            container.Verify();

            using (var scope = container.BeginExecutionContextScope())
            {
               var projectService = scope.GetInstance<IProjectService>();

                var project = new ProjectAddDto
                {
                    Name = "test",
                    ProjectItems = new List<ProjectItemAddDto>
                    {
                        new ProjectItemAddDto {Name = "车险", Price = 100}
                    }
                };

                //var projectId = projectService.Add(project);

                //Assert.IsTrue(projectId.IsNotBlank());
            }
        }

        /// <summary>
        /// RegisterForWebApiProxyClient
        /// </summary>
        /// <param name="container"></param>
        private static void RegisterService(Container container)
        {
            //dbcontext
            container.Register<IDbContextScopeFactory>(() => new DbContextScopeFactory(), Lifestyle.Scoped);

            //service
            var moduleInitializers = new ModuleInitializer[]
            {
                new BarryCESModuleInitializer()
            };

            moduleInitializers.ForEach(x => x.LoadIoc(container));

            //automapper
            container.Register<IConfigurationProvider>(AutoMapperConfig.GetMapperConfiguration, Lifestyle.Singleton);
            container.Register(() => AutoMapperConfig.GetMapperConfiguration().CreateMapper(), Lifestyle.Scoped);
        }
    }
}
