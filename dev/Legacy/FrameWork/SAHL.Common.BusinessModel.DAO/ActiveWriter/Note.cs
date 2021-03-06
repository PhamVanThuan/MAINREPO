﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SAHL.Common.BusinessModel {
    using System;
    using System.Collections.Generic;
    using System.Collections;
    using Castle.ActiveRecord;
    
    
    [ActiveRecord("Note", Schema="dbo")]
    public partial class Note : ActiveRecordBase<Note> {
        
        private int _noteKey;
        
        private int _genericKeyTypeKey;
        
        private int _genericKey;
        
        private IList<NoteDetail> _noteDetails;
        
        [PrimaryKey(PrimaryKeyType.Native, "NoteKey", ColumnType="Int32")]
        public virtual int NoteKey {
            get {
                return this._noteKey;
            }
            set {
                this._noteKey = value;
            }
        }
        
        [Property("GenericKeyTypeKey", ColumnType="Int32", NotNull=true)]
        public virtual int GenericKeyTypeKey {
            get {
                return this._genericKeyTypeKey;
            }
            set {
                this._genericKeyTypeKey = value;
            }
        }
        
        [Property("GenericKey", ColumnType="Int32", NotNull=true)]
        public virtual int GenericKey {
            get {
                return this._genericKey;
            }
            set {
                this._genericKey = value;
            }
        }
        
        [HasMany(typeof(NoteDetail), ColumnKey="NoteKey", Table="NoteDetail")]
        public virtual IList<NoteDetail> NoteDetails {
            get {
                return this._noteDetails;
            }
            set {
                this._noteDetails = value;
            }
        }
    }
    
    [ActiveRecord("NoteDetail", Schema="dbo")]
    public partial class NoteDetail : ActiveRecordBase<NoteDetail> {
        
        private int _noteDetailKey;
        
        private string _tag;
        
        private System.DateTime _tagDate;
        
        private string _workflowState;
        
        private System.DateTime _insertedDate;
        
        private string _noteText;
        
        private int _aDUserKey;
        
        private Note _note;
        
        [PrimaryKey(PrimaryKeyType.Native, "NoteDetailKey", ColumnType="Int32")]
        public virtual int NoteDetailKey {
            get {
                return this._noteDetailKey;
            }
            set {
                this._noteDetailKey = value;
            }
        }
        
        [Property("Tag", ColumnType="String")]
        public virtual string Tag {
            get {
                return this._tag;
            }
            set {
                this._tag = value;
            }
        }
        
        [Property("TagDate", ColumnType="Timestamp")]
        public virtual System.DateTime TagDate {
            get {
                return this._tagDate;
            }
            set {
                this._tagDate = value;
            }
        }
        
        [Property("WorkflowState", ColumnType="String")]
        public virtual string WorkflowState {
            get {
                return this._workflowState;
            }
            set {
                this._workflowState = value;
            }
        }
        
        [Property("InsertedDate", ColumnType="Timestamp", NotNull=true)]
        public virtual System.DateTime InsertedDate {
            get {
                return this._insertedDate;
            }
            set {
                this._insertedDate = value;
            }
        }
        
        [Property("Note", ColumnType="StringClob", NotNull=true)]
        public virtual string NoteText {
            get {
                return this._noteText;
            }
            set {
                this._noteText = value;
            }
        }
        
        [Property("ADUserKey", ColumnType="Int32", NotNull=true)]
        public virtual int ADUserKey {
            get {
                return this._aDUserKey;
            }
            set {
                this._aDUserKey = value;
            }
        }
        
        [BelongsTo("NoteKey")]
        public virtual Note Note {
            get {
                return this._note;
            }
            set {
                this._note = value;
            }
        }
    }
    
    public class NoteHelper {
        
        public static Type[] GetTypes() {
            return new Type[] {
                    typeof(Note),
                    typeof(NoteDetail)};
        }
    }
}
