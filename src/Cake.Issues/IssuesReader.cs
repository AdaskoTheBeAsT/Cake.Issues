﻿namespace Cake.Issues
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Diagnostics;
    using IssueProvider;

    /// <summary>
    /// Class for reading issues.
    /// </summary>
    public class IssuesReader
    {
        private readonly ICakeLog log;
        private readonly List<IIssueProvider> issueProviders = new List<IIssueProvider>();
        private readonly RepositorySettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="IssuesReader"/> class.
        /// </summary>
        /// <param name="log">Cake log instance.</param>
        /// <param name="issueProviders">List of issue providers to use.</param>
        /// <param name="settings">Settings to use.</param>
        public IssuesReader(
            ICakeLog log,
            IEnumerable<IIssueProvider> issueProviders,
            RepositorySettings settings)
        {
            log.NotNull(nameof(log));
            settings.NotNull(nameof(settings));

            // ReSharper disable once PossibleMultipleEnumeration
            issueProviders.NotNullOrEmptyOrEmptyElement(nameof(issueProviders));

            this.log = log;
            this.settings = settings;

            // ReSharper disable once PossibleMultipleEnumeration
            this.issueProviders.AddRange(issueProviders);
        }

        /// <summary>
        /// Read issues from issue providers.
        /// </summary>
        /// <param name="format">Preferred format for comments.</param>
        /// <returns>List of issues.</returns>
        public IEnumerable<IIssue> ReadIssues(IssueCommentFormat format)
        {
            // Initialize issue providers and read issues.
            var issues = new List<IIssue>();
            foreach (var issueProvider in this.issueProviders)
            {
                var providerName = issueProvider.GetType().Name;
                this.log.Verbose("Initialize issue provider {0}...", providerName);
                if (issueProvider.Initialize(this.settings))
                {
                    this.log.Verbose("Reading issues from {0}...", providerName);
                    var currentIssues = issueProvider.ReadIssues(format).ToList();

                    this.log.Verbose(
                        "Found {0} issues using issue provider {1}...",
                        currentIssues.Count,
                        providerName);

                    issues.AddRange(currentIssues);
                }
                else
                {
                    this.log.Warning("Error initializing issue provider {0}.", providerName);
                }
            }

            return issues;
        }
    }
}