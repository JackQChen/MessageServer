namespace SnControl
{
    using System;

    public class SolutionInstance
    {
        private static SolutionInstance m_Instance = null;
        private SnControl.Solution m_Solution;

        private SolutionInstance()
        {
        }

        public static SolutionInstance GetInstance()
        {
            if (m_Instance == null)
            {
                m_Instance = new SolutionInstance();
            }
            return m_Instance;
        }

        public SnControl.Solution Solution
        {
            get
            {
                return this.m_Solution;
            }
            set
            {
                this.m_Solution = value;
            }
        }
    }
}

