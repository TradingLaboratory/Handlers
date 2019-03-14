using System;
using System.Collections.Generic;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.DataSource;
using TSLab.Script.Helpers;
using System.ComponentModel; //чтобы работало "Описание
using TSLab.Script.Handlers.Options; //чтобы можно было указывать атрибуты класса "кубиков"

namespace TSLab.TraidingLaboratory.Indicators
{
    [HandlerCategory("TradingLaboratory - найденные")]

    //Название блока во вкладке "Подробно"

    [HelperName("BuyFractal (double)", Language = Constants.En)]
    [HelperName("Фрактал на покупку (double)", Language = Constants.Ru)]
    [HandlerName("Фрактал на покупку (double)")]

    //Описание во вкладке "Подробно" на руском и английском


    [Description("Фрактал на покупку – серия из N последовательных баров, в которой перед самым высоким максимумом и за ним находятся по (N-1)/2 бара с более низкими максимумами. " +
        "Имеет числовой выход. Отображается в виде кривой, значение меняется в момент срабатывания условия. " +
        "Left – количество баров слева до фрактала, Right – количество баров справа до фрактала" +
"CurrentBar – учитывать текущий i-ый бар при расчете: 0 – i-ый бар учитывается, 1 – i-ый бар НЕ учитывается" +
"Fractal – выдавать значение на баре с фракталом: 0 – значение выдается на текущем баре, 1 – значение выдается на баре с фракталом" +
"Обсуждение в Telegram -  канал Лаборатория Трейдинга ( http://t.me/TradingLaboratory )")] //на русском

    public class FractalBuyDouble : IBar2DoubleHandler, IContextUses
	{
		[HandlerParameter(true, "5", Min = "1", Max = "20", Step = "1", Name ="Свечей вправо")]
		public int Right { get; set; }
		
		[HandlerParameter(true, "5", Min = "1", Max = "20", Step = "5", Name ="Свечей влево")]
		public int Left { get; set; }
		
		[HandlerParameter(true, "0", Min = "0", Max = "1", Step = "1", Name ="Текущий бар")]
		public int CurrentBar { get; set; }
		
		[HandlerParameter(true, "0", Min = "0", Max = "1", Step = "1", Name ="Фрактал")]
		public int Fractal { get; set; }
		
		public IList<double> Execute(ISecurity symbol)
		{
		    var High = symbol.HighPrices;
		    IList<double> frbuy1 = new List<double>(High.Count);
		    IList<double> frbuy2 = new List<double>(High.Count);
		    double frbuyv =0;
		    double frbuyv1 =0;
		    int StartIndex=Left+(Right+CurrentBar);
		    int ResCheck=Left+Right;
		    
		    for (int i = 0; i < High.Count; i++)
		    {
		    	if(i<StartIndex)
		    		frbuyv=0;
		    	else
		    	{
		    	int Check = 0;
		    	int IndFr = i-(CurrentBar+Right);
		    	for(int j=(i-StartIndex);j<=(i-CurrentBar);j++)
		    		{
		    			if(j!=IndFr)
		    			{
		    				if(High[j]<High[IndFr])
		    					Check++;
		    				else
		    					break;
		    			}			    				
		    		}		    	
		    	if(Check==ResCheck)
		    		frbuyv=High[IndFr];
		    	}
		    	frbuy1.Add(frbuyv);
		    }
		    for (int i = (Right+CurrentBar); i < High.Count; i++)
		    {
		    	frbuyv1=frbuy1[i];
		    	frbuy2.Add(frbuyv1);
		    }
		    for (int i = High.Count-(Right+CurrentBar); i < High.Count; i++)
		    {
		    	frbuy2.Add(frbuyv1);
		    }
		    return Fractal==0?frbuy1:frbuy2;
		}		
		public IContext Context { get; set; }
	}
}
