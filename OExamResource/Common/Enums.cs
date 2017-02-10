using System;
using System.ComponentModel;

namespace Common
{
	public class Enums
	{
		public Enums()
		{
		}

		public enum BoutiqueGrade
		{
			[Description("预备")]
			Simple,
			[Description("普通")]
			Default,
			[Description("vip")]
			VIP,
			[Description("终身")]
			Life
		}

		public enum DbStatus
		{
			[Description("资源库")]
			Resource,
			[Description("医学库")]
			Medicine,
			[Description("精品课程库")]
			QualityCourse,
			[Description("试题库")]
			Test,
			[Description("模考库")]
			Exam,
			[Description("校内资源库")]
			Lan
		}

		public enum ExamOption
		{
			[Description("论述")]
			Text,
			[Description("判断")]
			Boolen,
			[Description("单选")]
			Single,
			[Description("多选")]
			Multiple,
			[Description("下拉")]
			Select
		}

		public enum LanResourceType
		{
			[Description("基本资源")]
			Base,
			[Description("扩展资源")]
			Ext,
			[Description("自定义资源")]
			Custom
		}

		public enum LogStatus
		{
			[Description("浏览")]
			Press,
			[Description("收藏")]
			Collect,
			[Description("下载")]
			DownLoad
		}

		public enum MedicalType
		{
			[Description("大类")]
			Category,
			[Description("学科")]
			Subject,
			[Description("专业")]
			Speciality
		}

		public enum PaperMeans
		{
			[Description("模拟")]
			Simulate,
			[Description("全真")]
			Past,
			[Description("导入")]
			Custom
		}

		public enum PaperTier
		{
			[Description("大类")]
			Level1,
			[Description("二级分类")]
			Level2,
			[Description("三级分类")]
			Level3
		}

		public enum QualityCourcetype
		{
			[Description("大类")]
			Subject,
			[Description("一级学科")]
			Subject1,
			[Description("二级学科")]
			Subject2
		}

		public enum ResourceType
		{
			[Description("大类")]
			Category,
			[Description("学科")]
			Subject,
			[Description("专业")]
			Speciality
		}

		public enum RoleType
		{
			[Description("总管理员")]
			Admin,
			[Description("学校管理员")]
			School,
			[Description("院系管理员")]
			Department,
			[Description("销售人员")]
			Sell,
			[Description("教师")]
			Teacher,
			[Description("学校可以登录后台")]
			SchoolManage
		}

		public enum SchoolTier
		{
			[Description("学校")]
			School,
			[Description("学院")]
			Faculty,
			[Description("系别")]
			Department
		}

		public enum TrialType
		{
			[Description("试用用户")]
			ShiYong = 0,
			[Description("镜像用户")]
			JingXiang = 1,
			[Description("下载超量用户")]
			SuperDown = 1
		}

		public enum UserType
		{
			[Description("管理员")]
			Admin = -1,
			[Description("教师")]
			Teacher = 0,
			[Description("普通用户")]
			Normal = 1,
			[Description("销售")]
			Sell = 3,
			[Description("学校可以登录后台")]
			SchoolManage = 5,
			[Description("学校后台管理员")]
			SchoolManageAdmin = 6
		}
	}
}