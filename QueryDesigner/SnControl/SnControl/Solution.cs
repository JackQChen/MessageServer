namespace SnControl
{
    using System;
    using System.Collections;

    [Serializable]
    public class Solution : ICloneable
    {
        private ArrayList dataSetList = new ArrayList();
        private string solutionComment;
        private string solutionID;
        private string solutionName;

        public object Clone()
        {
            Solution solution = new Solution();
            solution.dataSetList = (ArrayList) this.dataSetList.Clone();
            solution.solutionComment = this.solutionComment;
            solution.solutionID = this.solutionID;
            solution.solutionName = this.solutionName;
            solution.SolutionDataBaseType = SolutionDataBaseType;
            solution.SolutionConnectionString = SolutionConnectionString;
            return solution;
        }

        public ArrayList DataSetList
        {
            get
            {
                return this.dataSetList;
            }
            set
            {
                this.dataSetList = value;
            }
        }

        public string SolutionComment
        {
            get
            {
                return this.solutionComment;
            }
            set
            {
                this.solutionComment = value;
            }
        }

        public string SolutionID
        {
            get
            {
                return this.solutionID;
            }
            set
            {
                this.solutionID = value;
            }
        }

        public string SolutionName
        {
            get
            {
                return this.solutionName;
            }
            set
            {
                this.solutionName = value;
            }
        }

        public string SolutionDataBaseType
        {
            get;
            set;
        }

        public string SolutionConnectionString
        {
            get;
            set;
        }
    }
}

