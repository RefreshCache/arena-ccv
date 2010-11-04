using System;
using System.Data;
using System.Collections;
using System.Data.SqlClient;

using Arena.DataLayer;
using Arena.DataLib;

namespace Arena.Custom.CCV.DataLayer.HumanResources

{
	/// <summary>
	/// 
	/// </summary>
	public class StaffData : SqlData
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		public StaffData()
		{
		}

		/// <summary>
		/// Returns a <see cref="System.Data.SqlClient.SqlDataReader">SqlDataReader</see> object
		/// containing the Staff with the ID specified
		/// </summary>
		/// <param name="staffID">Staff ID</param>
		/// <returns><see cref="System.Data.SqlClient.SqlDataReader">SqlDataReader</see></returns>
		public SqlDataReader GetStaffByID(int personID)
		{
			ArrayList lst = new ArrayList();

			lst.Add(new SqlParameter("@PersonID", personID));

			try
			{
                return this.ExecuteSqlDataReader("cust_ccv_sp_hr_get_staffByID", lst);
			}
			catch (SqlException ex)
			{
				throw ex;
			}
			finally
			{
				lst = null;
			}
		}

        /// <summary>
        /// Returns a <see cref="System.Data.SqlClient.SqlDataReader">SqlDataReader</see> object
        /// containing the Staff with the GUID specified
        /// </summary>
        /// <param name="staffID">Staff GUID</param>
        /// <returns><see cref="System.Data.SqlClient.SqlDataReader">SqlDataReader</see></returns>
        public SqlDataReader GetStaffByGuid(Guid guid)
        {
            ArrayList lst = new ArrayList();

            lst.Add(new SqlParameter("@Guid", guid));

            try
            {
                return this.ExecuteSqlDataReader("cust_ccv_sp_hr_get_staffByGuid", lst);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                lst = null;
            }
        }
		
		public SqlDataReader GetStaffBySupervisorID(int supervisorID)
		{
			ArrayList lst = new ArrayList();

			lst.Add(new SqlParameter("@SupervisorID", supervisorID));

			try
			{
                return this.ExecuteSqlDataReader("cust_ccv_sp_hr_get_staffBySupervisorID", lst);
			}
			catch (SqlException ex)
			{
				throw ex;
			}
			finally
			{
				lst = null;
			}
		}

		public DataTable GetStaffBySupervisorID_DT(int supervisorID)
		{
			ArrayList lst = new ArrayList();

			lst.Add(new SqlParameter("@SupervisorID", supervisorID));

			try
			{
                return this.ExecuteDataTable("cust_ccv_sp_hr_get_staffBySupervisorID", lst);
			}
			catch (SqlException ex)
			{
				throw ex;
			}
			finally
			{
				lst = null;
			}
		}

        public SqlDataReader GetStaffByOrganizationID(int organizationID)
        {
            ArrayList lst = new ArrayList();

            lst.Add(new SqlParameter("@OrganizationID", organizationID));

            try
            {
                return this.ExecuteSqlDataReader("cust_ccv_sp_hr_get_staffByOrganizationID", lst);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                lst = null;
            }
        }

        /// <summary>
		/// deletes a Staff record.
		/// </summary>
		/// <param name="roleID">The poll_id key to delete.</param>
		public void DeleteStaff(int personID)
		{
			ArrayList lst = new ArrayList();

			lst.Add(new SqlParameter("@PersonID", personID));

			try
			{
                this.ExecuteNonQuery("cust_ccv_sp_hr_del_staff", lst);
			}
			catch (SqlException ex)
			{
				throw ex;
			}
			finally
			{
				lst = null;
			}

		}

		/// <summary>
		/// saves Staff record
		/// </summary>
		/// <returns><see cref="System.Data.SqlClient.SqlDataReader">SqlDataReader</see></returns>
		public void SaveStaff(int personID, int classLuid, int subDepartmentLuid, string title, DateTime hireDate, DateTime terminationDate, 
                              int terminationTypeLuid, int noticeTypeLuid, DateTime exitInterview, bool rehireEligible, DateTime rehireDate, string rehireNote, 
                              DateTime ministerDate, int badgeFob, DateTime badgeIssued, string keys, int supervisorID, int benefitEligibleLuid, 
                              DateTime benefitStartDate, DateTime benefitEndDate, int medicalChoiceLuid, int dentalChoiceLuid, int visionChoiceLuid, 
                              int lifeInsuranceClassLuid, bool retirementParticipant, Decimal retirementContribution, Decimal retirementMatch, 
                              decimal hsa, bool electCobra, DateTime cobraLetterSent, DateTime cobraEndDate, decimal fica, int weeklyHours, string userID)
		{
			ArrayList lst = new ArrayList();
			
			lst.Add(new SqlParameter("@PersonID", personID));
			lst.Add(new SqlParameter("@ClassLuid", classLuid));
			lst.Add(new SqlParameter("@SubDepartmentLuid", subDepartmentLuid));
			lst.Add(new SqlParameter("@Title", title));
			lst.Add(new SqlParameter("@HireDate", hireDate));
			lst.Add(new SqlParameter("@TerminationDate", terminationDate));
			lst.Add(new SqlParameter("@TerminationTypeLuid", terminationTypeLuid));
            lst.Add(new SqlParameter("@NoticeTypeLuid", noticeTypeLuid));
			lst.Add(new SqlParameter("@ExitInterview", exitInterview));
			lst.Add(new SqlParameter("@RehireEligible", rehireEligible));
			lst.Add(new SqlParameter("@RehireDate", rehireDate));
			lst.Add(new SqlParameter("@RehireNote", rehireNote));
			lst.Add(new SqlParameter("@MinisterDate", ministerDate));
			lst.Add(new SqlParameter("@BadgeFob", badgeFob));
			lst.Add(new SqlParameter("@BadgeIssued", badgeIssued));
			lst.Add(new SqlParameter("@Keys", keys));
			lst.Add(new SqlParameter("@SupervisorID", FKHelper.SaveForeignKey(supervisorID)));
			lst.Add(new SqlParameter("@BenefitEligibleLuid", benefitEligibleLuid));
			lst.Add(new SqlParameter("@BenefitStartDate", benefitStartDate));
			lst.Add(new SqlParameter("@BenefitEndDate", benefitEndDate));
			lst.Add(new SqlParameter("@MedicalChoiceLuid", medicalChoiceLuid));
			lst.Add(new SqlParameter("@DentalChoiceLuid", dentalChoiceLuid));
			lst.Add(new SqlParameter("@VisionChoiceLuid", visionChoiceLuid));
			lst.Add(new SqlParameter("@LifeInsuranceClassLuid", lifeInsuranceClassLuid));
			lst.Add(new SqlParameter("@RetirementParticipant", retirementParticipant));
			lst.Add(new SqlParameter("@RetirementContribution", retirementContribution));
			lst.Add(new SqlParameter("@RetirementMatch", retirementMatch));
			lst.Add(new SqlParameter("@Hsa", hsa));
			lst.Add(new SqlParameter("@ElectCobra", electCobra));
			lst.Add(new SqlParameter("@CobraLetterSent", cobraLetterSent));
			lst.Add(new SqlParameter("@CobraEndDate", cobraEndDate));
			lst.Add(new SqlParameter("@Fica", fica));
			lst.Add(new SqlParameter("@WeeklyHours", weeklyHours));
			lst.Add(new SqlParameter("@UserId", userID));
			
			
			try
			{
                this.ExecuteNonQuery("cust_ccv_sp_hr_save_staff", lst);
			}
			catch (SqlException ex)
			{
				throw ex;
			}
			finally
			{
				lst = null;
			}
		}
	}
}


