using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace craiglister
{
    class ReadHtml : IReadHtml
    {        
        public string Read(string url)
        {
            try {
                var request = (HttpWebRequest)WebRequest.Create(url);
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;

                    if (response.CharacterSet == null)
                    {
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    }

                    var data = readStream.ReadToEnd();

                    response.Close();
                    readStream.Close();

                    return data;
                }
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }    
    }
}
