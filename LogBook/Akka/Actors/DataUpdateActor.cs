using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Google.Protobuf.Reflection;

namespace LogBook.Akka.Actors
{
    class DataUpdateActor: UntypedActor
    {
        public class WriteMessage
        {
            public string[] FieldsToInsert { get; }

            public WriteMessage(string[] fieldsToInsert)
            {
                FieldsToInsert = fieldsToInsert;
            }
        }

        private DataTable _resultsTable;

        public DataUpdateActor(DataTable resultsTable)
        {
            _resultsTable = resultsTable;
        }

        protected override void OnReceive(object message)
        {
            if (message is WriteMessage update)
            {
                InsertFieldsToTable(update.FieldsToInsert);
            }
            else if (message is FileTailActor.GoNowAndDieInWhateverMannerSeemsBestToYou)
            {
                _resultsTable.Clear();
                _resultsTable = null;
                Context.Stop(Self);
            }
        }

        private void InsertFieldsToTable(string[] fieldsToInsert)
        {
            //DataRow row = _resultsTable.NewRow();
            //row.ItemArray = fieldsToInsert;
            //_resultsTable.Rows.InsertAt(row, 0);

            _resultsTable.Rows.Add(fieldsToInsert);
        }
    }
}
