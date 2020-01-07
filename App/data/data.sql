insert into entitymaster(name,text,tablename,primaryfield,namefield)
values('UserRole','UserRole','UserRole','id','name');

insert into entityschema(entityid,fieldname
,label,fieldtype,length,isrequired
,dbname,tablename
,viewtype,defaultvalue
,linkentity,linkentity_domain
,createdon,updatedon)
values(2,'id'
,'id',25,0,false
,'id','UserRole'
,0,null
,null,null
,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into entityschema(entityid,fieldname
,label,fieldtype,length,isrequired
,dbname,tablename
,viewtype,defaultvalue
,linkentity,linkentity_domain
,createdon,updatedon)
values(1,'AssignDate'
,'AssignDate',5,100,false
,'AssignDate','UserMaster'
,0,null
,null,null
,'2019-11-28 22:49:44','2019-11-28 22:49:44');

--link field
insert into entityschema(entityid,fieldname
,label,fieldtype,length,isrequired
,dbname,tablename
,viewtype,defaultvalue
,linkentity,linkentity_domain
,createdon,updatedon)
values(1,'Role'
,'Role',10,0,false
,'role__id','UserMaster'
,0,null
,2,null
,'2019-11-28 22:49:44','2019-11-28 22:49:44');