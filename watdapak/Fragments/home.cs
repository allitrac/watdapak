using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace watdapak.Fragments
{
    public class home : Fragment
    {
        private Button getProj;
        private Button getProjById;
        private Button addProj;
        private Button deleteProj;
        private Button checkOut;
        private Button checkIn;
        private Button updateProj, updateProjColl;
        private Button publishProj;
        private Button getTasks, addTasks, updateTask, deleteTask;
        private Button tsPeriod, tsCreate, tsSubmit, tsLines, tsAddLine, tsDeleteLine, tsGetLineWork, tsAddActualWork;
        private Button getER;
        private Button addResource, deleteER, updateER;
        private Button addAss;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
           View rootView =  inflater.Inflate(Resource.Layout.logged, container, false);
            MainActivity main = (Activity as MainActivity);

            getProj = rootView.FindViewById<Button>(Resource.Id.btnGetProjects);
            getProj.Click += delegate { main.GetProjects(); };

            getProjById = rootView.FindViewById<Button>(Resource.Id.btnGetProjectsById);
            getProjById.Click += delegate { main.getProjectEPT(); };

            addProj = rootView.FindViewById<Button>(Resource.Id.btnAddProject);
            addProj.Click += delegate { main.addProject(); };

            deleteProj = rootView.FindViewById<Button>(Resource.Id.btnDeleteProject);
            deleteProj.Click += delegate { main.deleteProject(); };

            checkOut = rootView.FindViewById<Button>(Resource.Id.btnCheckOutProject);
            checkOut.Click += delegate { main.checkOutProject(); };

            checkIn = rootView.FindViewById<Button>(Resource.Id.btnCheckInProject);
            checkIn.Click += delegate { main.checkInProject(); };

            updateProj = rootView.FindViewById<Button>(Resource.Id.btnUpdateProject);
            updateProj.Click += delegate { main.updateProject(); };

            updateProjColl = rootView.FindViewById<Button>(Resource.Id.btnUpdateProjectCollection);
            updateProjColl.Click += delegate { main.updateProjectCollection(); };

            publishProj = rootView.FindViewById<Button>(Resource.Id.btnPublishProject);
            publishProj.Click += delegate { main.publishProject(); };

            getTasks = rootView.FindViewById<Button>(Resource.Id.btnViewTasks);
            getTasks.Click += delegate { main.getTasks(); };

            addTasks = rootView.FindViewById<Button>(Resource.Id.btnAddTasks);
            addTasks.Click += delegate { main.addTask(); };

            updateTask = rootView.FindViewById<Button>(Resource.Id.btnUpdateTask);
            updateTask.Click += delegate { main.updateTask(); };

            deleteTask = rootView.FindViewById<Button>(Resource.Id.btnDeleteTask);
            deleteTask.Click += delegate { main.deleteTask(); };

            tsPeriod = rootView.FindViewById<Button>(Resource.Id.btnTimesheetPeriod);
            tsPeriod.Click += delegate { main.getTimesheetPeriod(); };

            tsCreate = rootView.FindViewById<Button>(Resource.Id.btnCreateTimesheet);
            tsCreate.Click += delegate { main.createTimesheet(); };

            tsSubmit = rootView.FindViewById<Button>(Resource.Id.btnSubmitTimesheet);
            tsSubmit.Click += delegate { main.submitTimesheet(); };

            tsLines = rootView.FindViewById<Button>(Resource.Id.btnTimesheetLines);
            tsLines.Click += delegate { main.getTimesheetLines(); };

            tsAddLine = rootView.FindViewById<Button>(Resource.Id.btnAddTimesheetLine);
            tsAddLine.Click += delegate { main.addTimesheetLine(); };

            tsDeleteLine = rootView.FindViewById<Button>(Resource.Id.btnDeleteTimesheetLine);
            tsDeleteLine.Click += delegate { main.deleteTimesheetLine(); };

            tsGetLineWork = rootView.FindViewById<Button>(Resource.Id.btnGetTimesheetLineWork);
            tsGetLineWork.Click += delegate { main.getTimesheetLineWork(); };

            tsAddActualWork = rootView.FindViewById<Button>(Resource.Id.btnAddActualWork);
            tsAddActualWork.Click += delegate { main.addTimesheetLineWork(); };

            getER = rootView.FindViewById<Button>(Resource.Id.btnGetER);
            getER.Click += delegate { main.getEnterpriseResources(); };

            addResource = rootView.FindViewById<Button>(Resource.Id.btnAddResource);
            addResource.Click += delegate { main.addEnterpriseResource(); };

            deleteER = rootView.FindViewById<Button>(Resource.Id.btnDeleteEnterpriseResource);
            deleteER.Click += delegate { main.deleteEnterpriseResource(); };

            updateER = rootView.FindViewById<Button>(Resource.Id.btnUpdateResource);
            updateER.Click += delegate { main.updateEnterpriseResource(); };

            addAss = rootView.FindViewById<Button>(Resource.Id.btnAddAssignment);
            addAss.Click += delegate { main.addAssignmentOnTask(); };




            return rootView;
        }
    }
}