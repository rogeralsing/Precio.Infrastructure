using System;
using System.Web;
using CodeBaseBlog.DomainModel;
using HibernatingRhinos.Profiler.Appender.EntityFramework;
using Precio.Domain;
using Precio.Transactional;

namespace CodeBaseBlog
{
    public class Global : HttpApplication
    {
        private void Application_Start(object sender, EventArgs e)
        {
            EntityFrameworkProfiler.Initialize();
            UoW.UoWFactory = () => new EFUoW(new MyBlogConnection());

            UoW.SubscribeToDomainEvents = () =>
                                              {
                                                  DomainEvents
                                                      .When<UserCommentedOnPost>()
                                                      .DelayUntill(CurrentTransaction.Committed)
                                                      .Then(@event => { });
                                              };
        }


        private void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown
        }

        private void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
        }

        private void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
        }

        private void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.
        }
    }
}