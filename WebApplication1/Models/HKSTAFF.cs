using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(HKSTAFFMetadata))]
    public partial class HKSTAFF
    {
        public IEnumerable<HKSTAFF> Descendent
        {
            // horrible routine - must fix it when time permits
            // recursive is bad should convert to loop instead
            // and a bad recursive routine is even worse
            // if the relation is screwed could cause infinite loop
            get
            {
                IList<HKSTAFF> myChildren = new List<HKSTAFF>();
                string staff = this.STAFFCODE;
                string child = "";
                string parent = "";

                foreach (HKSTAFFREL childrel in this.HKSTAFFRELs)
                {
                    child = childrel.CHILD;
                    parent = childrel.PARENT;

                    if (child == parent)
                    {
                        staff = childrel.HKSTAFF.STAFFCODE;
                        if (myChildren.IndexOf(childrel.HKSTAFF) < 0)
                        {
                            myChildren.Add(childrel.HKSTAFF);
                        }
                    }
                    else
                    {
                        var myDesc = childrel.HKSTAFF1.Descendent; // childs decendents

                        foreach (HKSTAFF items in myDesc)
                        {
                            staff = items.STAFFCODE;
                            if (myChildren.IndexOf(items) < 0)
                            {
                                myChildren.Add(items);
                            }
                        }
                    }
                }


                return myChildren;
            }
        }
    }

    public class HKSTAFFMetadata
    {
        [Display(Name = "Staff")]
        public string STAFFCODE { get; set; }

        [Display(Name = "Staff Name")]
        public string STAFFDESC { get; set; }

        [Display(Name = "Level")]
        public int STAFFLEVEL { get; set; }

        public string MSTRSTAFF { get; set; }
        public int REFCNT { get; set; }
        public int STAMP { get; set; }

    }
}