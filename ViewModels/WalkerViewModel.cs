using Honeycomb;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class WalkerViewModel:BaseViewModel
    {
        public ObservableCollection<string> ProgressMessages;
        
        public int Steps { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }
        public long Destinations { get; set; }
        public Honeycomb<long> Honeycomb { get; private set; }

        public WalkerViewModel()
        {
            ProgressMessages = new ObservableCollection<string>();
            Steps = 19;
            Honeycomb = ReadyHoneycombs.Hex19;
        }

        public async Task WalkAsync()
        {
            var walker = new Walker(Honeycomb, Steps);
            walker.CacheingData += SaveCacheingMessage;
            Cell<long> mostLikely = await walker.WalkAsync();

            Column = mostLikely.Column;
            Row = mostLikely.Row;
            Destinations = mostLikely.Data;
        }

        private void SaveCacheingMessage(object sender, Walker.CacheingEventArgs cea)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() => ProgressMessages.Insert(0,cea.Message));
        }
    }
}
