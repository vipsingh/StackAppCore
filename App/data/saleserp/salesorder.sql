
insert into t_entitymaster(masterid,id,name,text,tablename,primaryfield,namefield,appmodule,hasstages)
values(0,150,'SalesOrder','SalesOrder','t_salesorder','id', 'ordername',1,true);

insert into t_entity_itemtype(masterid,id,entityid,name,code)
values(0,10008,150,'Default','0');

insert into t_entity_viewlayout(masterid,id,entityid,itemtype,states,viewtype,layoutxml)
values(0,10008,150,10008,'','0', null);

insert into autoid values(150, 0);

insert into t_entitylist(masterid,id,entityid,categoryid,name,idfield,viewfield,
	layoutxml, 
	filterpolicy,fixedfilter,createdon,createdby) 
values(0,10008,150,0,'Default','id','name',
'<tlist type="GRID"><template><row><field id="ordername"/><field id="orderdate" /><field id="customer" /></row></template></tlist>',
'','', '2020-01-01',1);

create table t_salesorder
(
	masterid integer NOT NULL,
    id integer,	
	ordername character varying(100)  NOT NULL,
	itemtype int,
	orderdate date,
	remarks character varying(500)  NOT NULL,
	stageid int,
	statusid int,
	customer int,
    createdby integer NOT NULL,
    updatedby integer,
    createdon timestamp without time zone NOT NULL,	
    updatedon timestamp without time zone,
    CONSTRAINT t_salesorder_pkey PRIMARY KEY (id)
)


insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10046,150,'ordername','OrderName',1,100,true
,'ordername','t_salesorder',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10047,150,'orderdate','orderdate',5,0,true
,'orderdate','t_salesorder',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10048,150,'remarks','remarks',1,500,false
,'remarks','t_salesorder',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10049,150,'customer','customer',10,0,true
,'customer','t_salesorder',0,null
,111,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue,relatedexp
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10050,150,'addressline1','addressline1',10,0,false
,null,null,0,null,'customer.addressline1'
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue,relatedexp
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10051,150,'city','city',10,0,false
,null,null,0,null,'customer.city'
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue,linkentity_field
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10060,150,'items','Items',20,0,false
,null,null,0,null,'orderid'
,151,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

------------------------------------------------------------------------------


insert into t_entitymaster(masterid,id,name,text,tablename,primaryfield,namefield,appmodule,parententity)
values(0,151,'SalesOrderDetail','SalesOrderDetail','t_salesorder_detail','id', 'itemname',1,150);

insert into t_entity_viewlayout(masterid,id,entityid,itemtype,states,viewtype,layoutxml)
values(0,10009,151,0,'','0', null);

insert into autoid values(151, 0);

create table t_salesorder_detail
(
	masterid integer NOT NULL,
    id integer,	
	orderid int,
	itemid int not null,
	itemname character varying(100),
	description character varying(500),
	qty decimal(15,6),
	uom int,
	itemrate decimal(15,6),
	amount decimal(20,6),
    createdby integer NOT NULL,
    updatedby integer,
    createdon timestamp without time zone NOT NULL,	
    updatedon timestamp without time zone,
    CONSTRAINT t_salesorder_detail_pkey PRIMARY KEY (id)
)


insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10052,151,'orderid','OrderId',2,0,true
,'orderid','t_salesorder_detail',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10053,151,'itemid','itemid',10,0,true
,'itemid','t_salesorder_detail',0,null
,115,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue,relatedexp
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10054,151,'itemname','itemname',1,100,false
,'itemname','t_salesorder_detail',0,null,'itemid.name'
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10055,151,'description','description',2,0,false
,'description','t_salesorder_detail',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');


insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10056,151,'qty','qty',3,4,true
,'qty','t_salesorder_detail',0,'1'
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10057,151,'uom','uom',10,0,true
,'uom','t_salesorder_detail',0,null
,113,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10058,151,'itemrate','itemrate',3,4,true
,'itemrate','t_salesorder_detail',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue,computeexpression
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10059,151,'amount','amount',3,4,true
,'amount','t_salesorder_detail',0,null,'qty * itemrate'
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

-------------------------------------
insert into t_entitystatus_master(masterid,id,entityid,name,stageid,createdby,createdon)
values(0,101,150,'New',1,1,'2020-01-01');
insert into t_entitystatus_master(masterid,id,entityid,name,stageid,createdby,createdon)
values(0,102,150,'Approved',2,1,'2020-01-01');
insert into t_entitystatus_master(masterid,id,entityid,name,stageid,createdby,createdon)
values(0,103,150,'Invoiced',2,1,'2020-01-01');
insert into t_entitystatus_master(masterid,id,entityid,name,stageid,createdby,createdon)
values(0,104,150,'Closed',10,1,'2020-01-01');
insert into t_entitystatus_master(masterid,id,entityid,name,stageid,createdby,createdon)
values(0,105,150,'Rejected',9,1,'2020-01-01');