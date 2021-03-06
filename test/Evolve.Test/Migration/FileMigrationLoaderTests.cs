﻿using System.Collections.Generic;
using System.Linq;
using Evolve.Migration;
using Xunit;

namespace Evolve.Test.Migration
{
    [Collection("File migrations")]
    public class FileMigrationLoaderTests
    {
        [Fact]
        public void LoadFiles()
        {
            IList<IMigrationScript> scripts = new FileMigrationLoader().GetMigrations(new List<string> { "Resources/scripts." }, "V",
                "__", ".sql").ToList();
            Assert.Equal(2,scripts.Count);
            var script = scripts.First();
            Assert.Equal("V1_0_0__Test-Migration.sql", script.Name);
            Assert.Equal("Test-Migration", script.Description);
            Assert.NotNull(script.CheckSum);
            Assert.Equal("select 1;", script.LoadSqlStatements(null,"GO").FirstOrDefault());
        }

        [Fact]
        public void FailOnDuplicate()
        {
            Assert.Throws<EvolveConfigurationException>(() =>
                new EmbeddedResourceMigrationLoader(GetType().Assembly)
                    .GetMigrations(new List<string> { "Evolve.Test.Resources.scripts", "Evolve.Test.Resources.scripts2" },
                        "V", "__", ".sql").ToList());
        }
    }
}