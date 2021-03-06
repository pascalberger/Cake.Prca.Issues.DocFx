﻿namespace Cake.Prca.Issues.DocFx.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using Core.Diagnostics;
    using Core.IO;
    using Testing;

    internal class DocFxProviderFixture
    {
        public DocFxProviderFixture(string fileResourceName, DirectoryPath docRootPath)
        {
            this.Log = new FakeLog { Verbosity = Verbosity.Normal };

            using (var stream = this.GetType().Assembly.GetManifestResourceStream("Cake.Prca.Issues.DocFx.Tests.Testfiles." + fileResourceName))
            {
                using (var sr = new StreamReader(stream))
                {
                    this.Settings =
                        DocFxIssuesSettings.FromContent(
                            sr.ReadToEnd(),
                            docRootPath);
                }
            }

            this.PrcaSettings =
                new ReportIssuesToPullRequestSettings(@"c:\Source\Cake.Prca");
        }

        public FakeLog Log { get; set; }

        public DocFxIssuesSettings Settings { get; set; }

        public ReportIssuesToPullRequestSettings PrcaSettings { get; set; }

        public DocFxIssuesProvider Create()
        {
            var provider = new DocFxIssuesProvider(this.Log, this.Settings);
            provider.Initialize(this.PrcaSettings);
            return provider;
        }

        public IEnumerable<ICodeAnalysisIssue> ReadIssues()
        {
            var codeAnalysisProvider = this.Create();
            return codeAnalysisProvider.ReadIssues(PrcaCommentFormat.PlainText);
        }
    }
}
