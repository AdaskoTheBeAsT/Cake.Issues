﻿namespace Cake.Issues.Tests
{
    using Cake.Testing;
    using Shouldly;
    using Testing;
    using Xunit;

    public sealed class BaseIssueComponentTests
    {
        public sealed class TheCtor
        {
            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given / When
                var result = Record.Exception(() => new FakeIssueComponent(null));

                // Then
                result.IsArgumentNullException("log");
            }

            [Fact]
            public void Should_Set_Log()
            {
                // Given
                var log = new FakeLog();

                // When
                var provider = new FakeIssueComponent(log);

                // Then
                provider.Log.ShouldBe(log);
            }
        }

        public sealed class TheInitializeMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var provider = new FakeIssueComponent(new FakeLog());

                // When
                var result = Record.Exception(() => provider.Initialize(null));

                // Then
                result.IsArgumentNullException("settings");
            }

            [Fact]
            public void Should_Set_Settings()
            {
                // Given
                var provider = new FakeIssueComponent(new FakeLog());
                var settings = new RepositorySettings(@"c:\foo");

                // When
                provider.Initialize(settings);

                // Then
                provider.Settings.ShouldBe(settings);
            }

            [Fact]
            public void Should_Return_True()
            {
                // Given
                var provider = new FakeIssueComponent(new FakeLog());
                var settings = new RepositorySettings(@"c:\foo");

                // When
                var result = provider.Initialize(settings);

                // Then
                result.ShouldBe(true);
            }
        }
    }
}
