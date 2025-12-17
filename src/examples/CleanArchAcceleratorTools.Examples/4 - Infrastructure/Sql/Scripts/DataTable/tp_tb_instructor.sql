create type [dbo].[TP_TB_INSTRUCTOR] as table(
	[SQ_INSTRUCTOR] [bigint] NULL,
	[TX_FULL_NAME] [nvarchar](100) NULL,
	[DT_CREATION] [datetime] NULL
);