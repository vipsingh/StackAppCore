insert into entitymaster(name,text,tablename,primaryfield,namefield)
values('UserRole','UserRole','UserRole','id','name');

insert into entityschema(id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(12,2,'id','id',25,0,false
,'id','UserRole',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into entityschema(id,entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(11,1,'AssignDate','AssignDate',5,100,false
,'AssignDate','UserMaster',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

--select field
insert into entityschema(id,entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue,LookupId
,linkentity,linkentity_domain,createdon,updatedon)
values(14,1,'Type','Type',9,100,false
,'Type','UserMaster',0,null,1
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');


--link field
insert into entityschema(entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(1,'Role','Role',10,0,false
,'role__id','UserMaster',0,null
,2,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into entity_itemtype(id,entityid,name,code)
values(1,1,'Default','0');

insert into entity_viewlayout(id,entityid,itemtype,states,layoutxml)
values(1,1,1,'','<view entity="UserMaster">
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
						<field id="Password" />
						<field id="Role" domain="[[&quot;Category&quot;,0, &quot;@Type&quot;]]"/>
					</row>
					<row>
						<field id="Type" />
						<field id="RoleInfo" />
					</row>
					<row>
						<field id="SubmitAmount" invisible="[[&quot;Type&quot;,0, 2]]" />
						<field id="AssignDate" />
					</row>
			</group>
		</page>
	</pages>
	<commands>
		<!-- <command id="" text="Send" action_type="Redirect" action="sendToNext" actionParam="[{Name:&quot;xx&quot;, Source: {Type: &quot;QS&quot;, Name: &quot;Entity&quot;}}]" operation="" icon="" visibility="{}"/> -->
	</commands>
</view>');
