using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace craiglister
{
    class ReadWaiter : IReadHtml
    {
        private IReadHtml readHtml;
        private DateTime? lastCall;

        public ReadWaiter(IReadHtml readHtml)
        {
            this.readHtml = readHtml;
        }

        public string Read(string url)
        {
            if (lastCall.HasValue)
            {
                var timeLastSinceLastCall = (int)((DateTime.Now - lastCall.Value).TotalMilliseconds);
                if (timeLastSinceLastCall < 2000)
                    Thread.Sleep(2000 - timeLastSinceLastCall);
            }

            var result = readHtml.Read(url);

            lastCall = DateTime.Now;

            return result;
        }
    }
}
