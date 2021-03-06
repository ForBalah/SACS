﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using SACS.BusinessLayer.Views;

namespace SACS.BusinessLayer.Presenters
{
    /// <summary>
    /// Base class for presenters
    /// </summary>
    /// <typeparam name="T">Type of IViewBase</typeparam>
    public abstract class PresenterBase<T> where T : IViewBase
    {
        private T _View;

        /// <summary>
        /// Initializes a new instance of the <see cref="PresenterBase{T}"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public PresenterBase(T view)
        {
            this._View = view;
            this.Log = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// Gets the view associated with this presenter.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        protected T View
        {
            get
            {
                return this._View;
            }
        }

        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>
        /// The log.
        /// </value>
        protected ILog Log
        {
            get;
            private set;
        }

        /// <summary>
        /// Peforms the specified action, wrapped in a try/catch and informs the view of the outcome
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="exceptionAction">The exception action.</param>
        /// <param name="showError">If set to <c>true</c> show the error in the view.</param>
        protected void TryExecute(Action action, Action exceptionAction, bool showError)
        {
            try
            {
                ////Thread threadToKill = null;
                ////Action wrappedAction = () =>
                ////{
                ////    threadToKill = Thread.CurrentThread;
                ////    action();
                ////};

                ////IAsyncResult result = wrappedAction.BeginInvoke(null, null);
                ////if (result.AsyncWaitHandle.WaitOne(2000))
                ////{
                ////    wrappedAction.EndInvoke(result);
                ////}
                ////else
                ////{
                ////    throw new TimeoutException();
                ////}

                action();
            }
            catch (Exception e)
            {
                Exception miniException = new Exception(e.Message);
                this.Log.Error(e); // always log the original

                if (exceptionAction != null)
                {
                    try
                    {
                        exceptionAction();
                    }
                    catch (Exception ee)
                    {
                        miniException = new Exception(string.Format("{0}{1}{2}", e.Message, Environment.NewLine, ee.Message));
                        this.Log.Error(ee); // always log the original, not the mini
                    }
                }

                if (showError)
                {
                    this.View.ShowException("Error occured while contacting server", miniException);
                }
            }
        }
    }
}