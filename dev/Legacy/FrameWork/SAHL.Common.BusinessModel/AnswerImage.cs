using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AnswerImage_DAO
    /// </summary>
    public partial class AnswerImage : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AnswerImage_DAO>, IAnswerImage
    {
        public AnswerImage(SAHL.Common.BusinessModel.DAO.AnswerImage_DAO AnswerImage)
            : base(AnswerImage)
        {
            this._DAO = AnswerImage;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AnswerImage_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AnswerImage_DAO.Answer
        /// </summary>
        public IAnswer Answer
        {
            get
            {
                if (null == _DAO.Answer) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IAnswer, Answer_DAO>(_DAO.Answer);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Answer = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Answer = (Answer_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AnswerImage_DAO.Image
        /// </summary>
        public IImage Image
        {
            get
            {
                if (null == _DAO.Image) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IImage, Image_DAO>(_DAO.Image);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Image = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Image = (Image_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}