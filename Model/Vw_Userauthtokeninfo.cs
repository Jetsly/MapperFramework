/*
<auto-generated>
 此代码由T4模板自动生成
 生成时间 2014-10-29 22:20:33 by Auto Generated
*/
namespace Model
{
	using System;

    /// <summary>
    /// VIEW
    /// </summary>
	public partial class Vw_Userauthtokeninfo :DBModel<Vw_Userauthtokeninfo>
	{ 
     
       /// <summary>
	   /// 真实名字
	   /// </summary>
       private string _realname;
	   /// <summary>
	   /// 真实名字
	   /// </summary>
	   public string RealName 
	   { 
	      get { return _realname; }
          set { SetWithNotify(value, ref _realname); }	   
	   }			  
     
       /// <summary>
	   /// 部门名称
	   /// </summary>
       private string _deptname;
	   /// <summary>
	   /// 部门名称
	   /// </summary>
	   public string DeptName 
	   { 
	      get { return _deptname; }
          set { SetWithNotify(value, ref _deptname); }	   
	   }			  
     
       /// <summary>
	   /// 父部门名称
	   /// </summary>
       private string _parentdeptname;
	   /// <summary>
	   /// 父部门名称
	   /// </summary>
	   public string ParentDeptName 
	   { 
	      get { return _parentdeptname; }
          set { SetWithNotify(value, ref _parentdeptname); }	   
	   }			  
     
       /// <summary>
	   /// 账号状态
	   /// </summary>
       private int? _accountstatusid;
	   /// <summary>
	   /// 账号状态
	   /// </summary>
	   public int? AccountStatusId 
	   { 
	      get { return _accountstatusid; }
          set { SetWithNotify(value, ref _accountstatusid); }	   
	   }			  
     
       /// <summary>
	   /// 手机号
	   /// </summary>
       private string _mobilephone;
	   /// <summary>
	   /// 手机号
	   /// </summary>
	   public string mobilePhone 
	   { 
	      get { return _mobilephone; }
          set { SetWithNotify(value, ref _mobilephone); }	   
	   }			  
     
       /// <summary>
	   /// 登陆授权的token
	   /// </summary>
       private string _authtoken;
	   /// <summary>
	   /// 登陆授权的token
	   /// </summary>
	   public string AuthToken 
	   { 
	      get { return _authtoken; }
          set { SetWithNotify(value, ref _authtoken); }	   
	   }			  
     
       /// <summary>
	   /// 家庭地址
	   /// </summary>
       private string _homeaddress;
	   /// <summary>
	   /// 家庭地址
	   /// </summary>
	   public string HomeAddress 
	   { 
	      get { return _homeaddress; }
          set { SetWithNotify(value, ref _homeaddress); }	   
	   }			  
     
       /// <summary>
	   /// 性别
	   /// </summary>
       private string _sex;
	   /// <summary>
	   /// 性别
	   /// </summary>
	   public string Sex 
	   { 
	      get { return _sex; }
          set { SetWithNotify(value, ref _sex); }	   
	   }			  
     
       /// <summary>
	   /// 用户邮件
	   /// </summary>
       private string _usermail;
	   /// <summary>
	   /// 用户邮件
	   /// </summary>
	   public string UserMail 
	   { 
	      get { return _usermail; }
          set { SetWithNotify(value, ref _usermail); }	   
	   }			  
     
       /// <summary>
	   /// 职位
	   /// </summary>
       private string _position;
	   /// <summary>
	   /// 职位
	   /// </summary>
	   public string Position 
	   { 
	      get { return _position; }
          set { SetWithNotify(value, ref _position); }	   
	   }			  
     
       /// <summary>
	   /// 办公电话
	   /// </summary>
       private string _officephone;
	   /// <summary>
	   /// 办公电话
	   /// </summary>
	   public string OfficePhone 
	   { 
	      get { return _officephone; }
          set { SetWithNotify(value, ref _officephone); }	   
	   }			  
     
       /// <summary>
	   /// 头像
	   /// </summary>
       private string _headpic;
	   /// <summary>
	   /// 头像
	   /// </summary>
	   public string HeadPic 
	   { 
	      get { return _headpic; }
          set { SetWithNotify(value, ref _headpic); }	   
	   }			  
     
       /// <summary>
	   /// 用户名
	   /// </summary>
       private string _loginname;
	   /// <summary>
	   /// 用户名
	   /// </summary>
	   public string LoginName 
	   { 
	      get { return _loginname; }
          set { SetWithNotify(value, ref _loginname); }	   
	   }			  
     
       /// <summary>
	   /// 登录名
	   /// </summary>
       private string _anothername;
	   /// <summary>
	   /// 登录名
	   /// </summary>
	   public string AnotherName 
	   { 
	      get { return _anothername; }
          set { SetWithNotify(value, ref _anothername); }	   
	   }			  
		
	}

}
