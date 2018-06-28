using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Blockz_vs_Ballz.Scripts
{
    class SkinObject
    {
        private string mNameSkin;
        private int mCost;
        private string mImageSkin;
        private string mHeadSkin;
        private string mBodySkin;
        private int mStatusEquid;
        private int mStatusBuy;
        public SkinObject()
        {
           
        }
        public SkinObject(string mNameSkin, int mCost, string mImageSkin, string mHeadSkin, string mBodySkin, int mStatusBuy, int mStatusEquid)
        {
            this.mNameSkin = mNameSkin;
            this.mCost = mCost;
            this.mImageSkin = mImageSkin;
            this.mHeadSkin = mHeadSkin;
            this.mBodySkin = mBodySkin;
            this.mStatusEquid = mStatusEquid;
            this.mStatusBuy = mStatusBuy;
        }

        public string NameSkin
        {
            get
            {
                return mNameSkin;
            }

            set
            {
                mNameSkin = value;
            }
        }

        public int Cost
        {
            get
            {
                return mCost;
            }

            set
            {
                mCost = value;
            }
        }

        public String ImageSkin
        {
            get
            {
                return mImageSkin;
            }

            set
            {
                mImageSkin = value;
            }
        }

        public string HeadSkin
        {
            get
            {
                return mHeadSkin;
            }

            set
            {
                mHeadSkin = value;
            }
        }

        public string BodySkin
        {
            get
            {
                return mBodySkin;
            }

            set
            {
                mBodySkin = value;
            }
        }

        public int StatusEquid
        {
            get
            {
                return mStatusEquid;
            }

            set
            {
                mStatusEquid = value;
            }
        }

        public int StatusBuy
        {
            get
            {
                return mStatusBuy;
            }

            set
            {
                mStatusBuy = value;
            }
        }
    }
}
