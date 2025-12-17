select
	*
from
	TB_COURSE a
left join
	TB_INSTRUCTOR b on a.SQ_INSTRUCTOR = b.SQ_INSTRUCTOR
left join
	TB_PROFILE c on b.SQ_INSTRUCTOR = c.SQ_INSTRUCTOR
left join
	TB_ADDRESS d on c.SQ_ADDRESS = d.SQ_ADDRESS
left join
	TB_MODULE e on a.SQ_COURSE = e.SQ_COURSE
left join
	TB_LESSON f on e.SQ_MODULE = f.SQ_MODULE;

select * from TB_COURSE;
select * from TB_MODULE;
select * from TB_LESSON;
select * from TB_INSTRUCTOR;
select * from TB_PROFILE;
select * from TB_ADDRESS;