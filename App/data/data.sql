---------Test Entity

CREATE TABLE t_test_entity
(
    masterid integer NOT NULL,
    id integer,	
	name character varying(100)  NOT NULL,
    image character varying(1000),
	areas int[],
	itemtype int,
    createdby integer NOT NULL,
    updatedby integer,
    createdon timestamp without time zone NOT NULL,	
    updatedon timestamp without time zone,
    CONSTRAINT t_test_entity_pkey PRIMARY KEY (id)
);

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,990,99,'name','Name',1,100,true
,'name','t_test_entity',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,991,99,'image','Image',14,0,false
,'image','t_test_entity',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,994,99,'areas','areas',11,100,false
,'areas','t_test_entity',0,null
,131,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entitymaster(masterid,id,name,text,tablename,primaryfield,namefield)
values(0,99,'TestEntity','TestEntity','t_test_entity','id', 'name');

insert into t_entity_itemtype(masterid,id,entityid,name,code)
values(0,999,99,'Default','0');

insert into t_entity_viewlayout(masterid,id,entityid,itemtype,states,viewtype,layoutxml)
values(0,999,99,999,'','0', null);

insert into autoid values(99, 0);

delete from t_entitylist where id=999;
insert into t_entitylist(masterid,id,entityid,categoryid,name,idfield,viewfield,
	layoutxml, 
	filterpolicy,fixedfilter,createdon,createdby) 
values(0,999,99,0,'Default','id','name',
'<tlist type="GRID"><template><row><field id="name"/><field id="image" widget="11" format="" width="" style="" /><field id="category" link="true" /><field id="areas" /></row></template><rules><rule type="conditionalformat" criteria="[{&quot;category&quot;:[0, 1]}]" style="{&quot;color&quot; : &quot;red&quot;}" /></rules></tlist>',
'[{"category": [0,0]}]','', '2020-01-01',1);

--------------OneToMany Field
insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,linkentity_field, createdon,updatedon)
values(0,993,99,'categories','Categories',20,0,false
,null,null,0,null
,112,null,'testobjid','2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10035,112,'testobjid','testobjid',2,4,false
,'testobjid','"t_product_category"',-1,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

alter table t_product_category add testobjid integer

---------------------------------------------


--entity action definition
insert into t_entityactions(id,entityid,viewtype,actiontype,text
,action,queryparam,
dataparam,createdon) 
values(10002,101,2,23,'Fun 1'
,'SendMail','[{"Name":"EntityId","Value":2},{"Name":"ItemId","Value":"@Role"}]',
'[{"Name":"EmailId","Value":"x@x.co"},{"Name":"Template","Value":"1"}]','2020-01-01');


update entity_viewlayout set layoutxml= '<view entity="UserMaster">
	<header>
		<group>
			<row>
				<field text="Name" id="Name" />
				<field id="LoginId" />
			</row>
		</group>
	</header>
	<pages display="TAB">
		<page text="general">
			<group text="section 1">
				<row>
				    <field id="AssignDate" />
					<field id="Role" domain="[{&quot;Category&quot;:[0, &quot;@Type&quot;]}]"/>
				</row>
				<row>
					<field id="Type" />
					<field id="rolename" />
				</row>
				<row>
					<field id="SubmitAmount" hidden="[{&quot;Type&quot;:[0, 2]}]" />
					<field id="TotalAmount" />
				</row>
				<row>
					<field id="Addresses" />
				</row>
			</group>
		</page>
	</pages>
	<commands>
		<command id="ACT_ENTITY_10001" text="View Role" />
		<command id="ACT_ENTITY_10002" text="Call FUN" />
	</commands>
	<rules>
	    <rule type="hidden" criteria="[{&quot;Type&quot;:[0, 2]}]" fields="SubmitAmount" />
	    <rule type="readonly" criteria="[{&quot;Type&quot;:[0, 2]}]" fields="Role" />
	</rules>
</view>' where id=10001;