--drop table if exists usermaster CASCADE;
insert into t_entitymaster(masterid,id,name,text,tablename,primaryfield,namefield)
values(0,101,'User','User','t_user','id', 'name');

insert into t_entity_itemtype(masterid,id,entityid,name,code)
values(0,10001,101,'Default','0');

insert into t_entity_viewlayout(masterid,id,entityid,itemtype,states,viewtype,layoutxml)
values(0,10001,101,10001,'','0', null);

insert into autoid values(101, 0);


insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10036,101,'name','Name',1,100,true
,'name','t_user',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10037,101,'shortname','shortname',1,60,false
,'shortname','t_user',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10042,101,'mobileno','mobileno',28,20,false
,'mobileno','t_user',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10043,101,'emailid','emailid',15,60,true
,'emailid','t_user',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10045,101,'expiredon','expiredon',5,0,false
,'expiredon','t_user',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

---------------------------------------------------------------------------------------

insert into t_entitymaster(masterid,id,name,text,tablename,primaryfield,namefield)
values(0,102,'UserRole','UserRole','t_role','id', 'name');

insert into t_entity_itemtype(masterid,id,entityid,name,code)
values(0,10007,102,'Default','0');

insert into t_entity_viewlayout(masterid,id,entityid,itemtype,states,viewtype,layoutxml)
values(0,10007,102,10007,'','0', null);

insert into autoid values(102, 1);
--------------------------------------------------------------------------------------------

--Drop all

select 'drop table "' || tablename || '" cascade;' from pg_tables where schemaname = 'public';

drop function get_collection_datatext;
drop function get_related_data;