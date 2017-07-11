using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading;

namespace Trans

{
    [Activity(Label = "История переводов")]
    public class SeeHistory : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.History);

            var progressDialog = ProgressDialog.Show(this, "Пожалуйста, подождите..", "Загрузка истории..", true);
            progressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);

            new Thread(new ThreadStart(delegate
            {
                Thread.Sleep(2000);
                var hist = DataSQLDroid.DataRead();
                hist = hist.OrderByDescending(x => x.transData).Where(x => x.isFaforite==0).ToList();
               
                RunOnUiThread(() => {
                    var listView = FindViewById<ListView>(Resource.Id.listView);
                    listView.Adapter = new TransAdapter(this, hist);
                    progressDialog.Dismiss();
                });
                

            })).Start();



            // Create your application here
        }
    }
}