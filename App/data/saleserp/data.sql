insert into t_city_area values(0,1,'Ace City', 1, '201306', 1,null, '2020-01-01', null);
insert into t_city_area values(0,2,'Stellar Jeevan', 1, '201306', 1,null, '2020-01-01', null);
update autoid set maxid=2;


insert into t_collection_master values(0,2,1,'Gram',null,'Gm',null,null);
insert into t_collection_master values(0,2,2,'Milileter',null,'Ml',null,null);
insert into t_collection_master values(0,2,3,'Paket',null,'Pkt',null,null);

insert into t_entityactions(masterid,id,entityid,viewtype,text,actiontype,action,queryparam,dataparam,createdon)
values(0,11,1,2,'Studio',17, 'studio/studio', '[{"Name":"EntityId","Value":"@$qs.ItemId"}]', null,'2020-01-01');

update t_entity_viewlayout set layoutxml='<view entity="entityschema"><header><group><row><field text="Name" id="Name" /></row></group></header><pages display="TAB"><page text="general"><group text="section 1"><row><field id="text" /><field id="tablename"/></row><row><field id="primaryfield" /><field id="namefield" /></row></group></page></pages><commands><command id="11" text="Studio" /></commands><rules><rule type="readonly" criteria="[{&quot;Id&quot;:[3, 0]}]" fields="tablename" /></rules></view>' where id=1;
