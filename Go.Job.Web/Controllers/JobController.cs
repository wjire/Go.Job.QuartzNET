﻿using Go.Job.Db;
using Go.Job.Model;
using Go.Job.Web.Helper;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Go.Job.Web.Controllers
{
    public class JobController : Controller
    {
        public ActionResult Index()
        {
            List<JobInfo> list = JobInfoDb.GetJobInfoList();
            return View(list);
        }


        public ActionResult Add(JobInfo jobInfo)
        {
            JobInfoDb.AddJobInfo(jobInfo);
            return RedirectToAction("Index", "Job");
        }

        public ActionResult Run(int id)
        {
            JobHelper.Run(id);
            return RedirectToAction("Index", "Job");
        }

        public ActionResult Pause(int id)
        {
            JobHelper.Pause(id);
            return RedirectToAction("Index", "Job");
        }

        public ActionResult Resume(int id)
        {
            JobHelper.Resume(id);
            return RedirectToAction("Index", "Job");
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            JobInfo jobInfo = JobInfoDb.GetJobInfo(id);
            return Json(jobInfo, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UpdateJob(JobInfo jobInfo)
        {
            JobHelper.Update(jobInfo);
            return RedirectToAction("Index", "Job");
        }


        public ActionResult Remove(int id)
        {
            JobHelper.Remove(id);
            return RedirectToAction("Index", "Job");
        }
    }
}