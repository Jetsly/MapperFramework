/*
<auto-generated>
 此代码由T4模板自动生成
 生成时间 2014-10-29 22:20:33 by Auto Generated
*/
namespace Model
{
	using System;

    /// <summary>
    /// 
    /// </summary>
	public partial class Group :DBModel<Group>
	{ 
     
       /// <summary>
	   /// 群组的Id
	   /// </summary>
       private int _groupid;
	   /// <summary>
	   /// 群组的Id
	   /// </summary>
	   public int GroupId 
	   { 
	      get { return _groupid; }
          set { SetWithNotify(value, ref _groupid); }	   
	   }			  
     
       /// <summary>
	   /// 群组的名称
	   /// </summary>
       private string _groupname;
	   /// <summary>
	   /// 群组的名称
	   /// </summary>
	   public string GroupName 
	   { 
	      get { return _groupname; }
          set { SetWithNotify(value, ref _groupname); }	   
	   }			  
     
       /// <summary>
	   /// 群组的头像
	   /// </summary>
       private string _groupheadpic;
	   /// <summary>
	   /// 群组的头像
	   /// </summary>
	   public string GroupHeadPic 
	   { 
	      get { return _groupheadpic; }
          set { SetWithNotify(value, ref _groupheadpic); }	   
	   }			  
     
       /// <summary>
	   /// 群组的最大成员数
	   /// </summary>
       private int _groupmax;
	   /// <summary>
	   /// 群组的最大成员数
	   /// </summary>
	   public int GroupMax 
	   { 
	      get { return _groupmax; }
          set { SetWithNotify(value, ref _groupmax); }	   
	   }			  
     
       /// <summary>
	   /// 群组的创建时间
	   /// </summary>
       private DateTime? _createdate;
	   /// <summary>
	   /// 群组的创建时间
	   /// </summary>
	   public DateTime? CreateDate 
	   { 
	      get { return _createdate; }
          set { SetWithNotify(value, ref _createdate); }	   
	   }			  
     
       /// <summary>
	   /// 群组的时间戳
	   /// </summary>
       private DateTime? _timestamp;
	   /// <summary>
	   /// 群组的时间戳
	   /// </summary>
	   public DateTime? TimeStamp 
	   { 
	      get { return _timestamp; }
          set { SetWithNotify(value, ref _timestamp); }	   
	   }			  
		
	}

}
