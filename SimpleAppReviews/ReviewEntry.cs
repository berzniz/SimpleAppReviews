using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleAppReviews
{
    class ReviewEntry
    {
        public string storeId;
        public string reviewTitle;
        public string reviewStars;
        public string reviewUser;
        public string reviewVersion;
        public string reviewDate;
        public string reviewText;

        override public string ToString()
        {
            return reviewUser + ": " + reviewVersion + "\r\n" + reviewDate + "\r\n" + reviewTitle + "\r\n" + reviewStars + "\r\n" + reviewText + "\r\n";
        }
    }
}
