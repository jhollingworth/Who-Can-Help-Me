namespace MSpecTests.WhoCanHelpMe.Tasks
{
    #region Using Directives

    using System.Collections.Generic;

    using global::WhoCanHelpMe.Domain;
    using global::WhoCanHelpMe.Domain.Contracts.Configuration;
    using global::WhoCanHelpMe.Domain.Contracts.Tasks;
    using global::WhoCanHelpMe.Tasks;

    using Machine.Specifications;
    using Machine.Specifications.AutoMocking.Rhino;
    using Rhino.Mocks;

    #endregion

    public class specification_for_site_metadata_tasks : Specification<ISiteMetaDataTasks, SiteMetaDataTasks>
    {
        protected static IConfigurationService the_configuration_service;

        Establish context = () =>
        {
            the_configuration_service = DependencyOf<IConfigurationService>();
        };
    }

    [Subject(typeof(SiteMetaDataTasks))]
    public class when_the_site_metadata_tasks_is_asked_to_get_metadata : specification_for_site_metadata_tasks
    {
        protected static IAnalyticsConfiguration the_analytics_configuration;

        private const string analytics_identifier = "the identifier";
        private const string site_verification = "site verification";
        private const string site_title = "Who Can Help Me?";

        private static SiteMetaData result;

        Establish context = () =>
        {
            the_analytics_configuration = MockRepository.GenerateMock<IAnalyticsConfiguration>();

            the_analytics_configuration.Stub(x => x.Idenfitier).Return(analytics_identifier);
            the_analytics_configuration.Stub(x => x.Verification).Return(site_verification);

            the_configuration_service.Stub(x => x.Analytics).Return(the_analytics_configuration);
        };

        Because of = () => result = subject.GetSiteMetaData();

        It should_return_site_metadata = () => result.ShouldNotBeNull();

        It should_return_the_correct_analytics_identifier = () => result.AnalyticsIdentifier.ShouldEqual(analytics_identifier);
        
        It should_return_the_correct_site_verification = () => result.SiteVerification.ShouldEqual(site_verification);

        It should_return_the_correct_site_title = () => result.Title.ShouldEqual(site_title);

        It should_return_the_list_of_scripts_to_include = () => result.Scripts.Count.ShouldBeGreaterThan(0);

        It should_return_the_list_of_stylesheets_to_include= () => result.Styles.Count.ShouldBeGreaterThan(0);
    }
}