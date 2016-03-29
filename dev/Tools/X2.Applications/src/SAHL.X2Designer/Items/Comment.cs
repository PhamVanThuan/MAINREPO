using System;
using System.ComponentModel;
using System.Windows.Forms;
using Northwoods.Go;

namespace SAHL.X2Designer.Items
{
    /// <summary>
    /// 1) WorkList
    /// </summary>

    [Serializable]
    public class Comment : BaseItem
    {
        private int m_CommentID;
        private string m_Comment = "";

        public Comment()
        {
            m_ItemType = WorkflowItemType.Comment;
            m_ItemBaseType = WorkflowItemBaseType.Comment;
            m_IconName = "";
        }

        public int CommentID
        {
            get
            {
                return m_CommentID;
            }
            set
            {
                m_CommentID = value;
            }
        }

        public string CommentText
        {
            get
            {
                return m_Comment;
            }
            set
            {
                m_Comment = value;
            }
        }

        public override object Properties
        {
            get
            {
                return new CommentProperties(this);
            }
        }
    }

    [Serializable]
    public class CommentProperties : BaseProperties
    {
        public CommentProperties(BaseItem Owner)
            : base(Owner)
        {
        }

        [Description("The name of the comment"), Category(CommonProps.GeneralProps)]
        public int CommentID
        {
            get
            {
                return ((Comment)m_Owner).CommentID;
            }
        }

        [Description("Comment Text"), Category(CommonProps.CommentProps)]
        public string CommentText
        {
            get
            {
                return ((Comment)m_Owner).Name;
            }
            set
            {
                bool found = false;
                if (MainForm.App.GetCurrentView() != null)
                {
                    foreach (GoObject o in MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
                    {
                        Comment mComment = o as Comment;
                        if (mComment != null)
                        {
                            if (mComment.Name == value)
                            {
                                found = true;
                            }
                        }
                    }
                }
                if (found == false)
                {
                    ((Comment)m_Owner).Name = value;
                }
                else
                {
                    MessageBox.Show("A Comment with this name already exists!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }
    }
}