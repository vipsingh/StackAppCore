insert into entitymaster(name,text,tablename,primaryfield,namefield)
values('UserRole','UserRole','UserRole','id','name');
---------------------------
insert into entityschema(id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(11,2,'id','id',25,0,true
,'id','UserRole',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into entityschema(id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(12,2,'name','Name',1,100,true
,'name','UserRole',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');
----------------------------------
CREATE TABLE userrole
(
    id integer NOT NULL,
    name character varying(100) NOT NULL,        
    createdon timestamp without time zone,
    updatedon timestamp without time zone,
    CONSTRAINT userrole_pkey PRIMARY KEY (id)
)
--------------------------------------
insert into entity_itemtype(id,entityid,name,code)
values(1,2,'Default','0');

insert into entity_viewlayout(id,entityid,itemtype,states,viewtype,layoutxml)
values(2,2,2,'','0', null);
--------------------------------------