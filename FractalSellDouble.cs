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

    [HelperName("SellFractal (double)", Language = Constants.En)]
    [HelperName("Фрактал на продажу (double)", Language = Constants.Ru)]
    [HandlerName("Фрактал на продажу (double)")]

    //Описание во вкладке "Подробно" на руском и английском
    [Description("Фрактал на продажу – серия из N последовательных баров, в которой перед самым низким минимумом и за ним находятся по (N-1)/2 бара с более высокими минимумами. " +
          "Имеет числовой выход. Отображается в виде кривой, значение меняется в момент срабатывания условия. " +
          "Left – количество баров слева до фрактала, Right – количество баров справа до фрактала" +
  "CurrentBar – учитывать текущий i-ый бар при расчете: 0 – i-ый бар учитывается, 1 – i-ый бар НЕ учитывается" +
  "Fractal – выдавать значение на баре с фракталом: 0 – значение выдается на текущем баре, 1 – значение выдается на баре с фракталом" +
  "Обсуждение в Telegram -  канал Лаборатория Трейдинга ( http://t.me/TradingLaboratory )")] //на русском
    public class FractalSellDouble : IBar2DoubleHandler, IContextUses
	{
		[HandlerParameter(true, "5", Min = "1", Max = "20", Step = "1", Name ="Свечей вправо")]
		public int Right { get; set; }
		
		[HandlerParameter(true, "5", Min = "1", Max = "20", Step = "5", Name ="Свечей влево")]
		public int Left { get; set; }
		
		[HandlerParameter(true, "0", Min = "0", Max = "1", Step = "1", Name ="Текущий бар")]
		public int CurrentBar { get; set; }
		
		[HandlerParameter(true, "0", Min = "0", Max = "1", Step = "1", Name ="Фрактал")]
		public int Fractal { get; set; }
		
		public IList<double> Execute(ISecurity source)
		{
		    var Low = source.LowPrices;
		    IList<double> frsell1 = new List<double>(Low.Count);
		    IList<double> frsell2 = new List<double>(Low.Count);
		    double frsellv =0;
			double frsellv1 =0;
		    int StartIndex=Left+(Right+CurrentBar);
		    int ResCheck=Left+Right;
		    
		    for (int i = 0; i < Low.Count; i++)
		    {
		    	if(i<StartIndex)
		    		frsellv=0;
		    	else
		    	{
		    	int Check = 0;
		    	int IndFr = i-(CurrentBar+Right);
		    	for(int j=(i-StartIndex);j<=(i-CurrentBar);j++)
		    		{
		    			if(j!=IndFr)
		    			{
		    				if(Low[j]>Low[IndFr])
		    					Check++;
		    				else
		    					break;
		    			}			    				
		    		}		    	
		    	if(Check==ResCheck)
		    		frsellv=Low[IndFr];
		    	}
		    	frsell1.Add(frsellv);
		    }
		    for (int i = (Right+CurrentBar); i < Low.Count; i++)
		    {
		    	frsellv1=frsell1[i];
		    	frsell2.Add(frsellv1);
		    }
		    for (int i = Low.Count-(Right+CurrentBar); i < Low.Count; i++)
		    {
		    	frsell2.Add(frsellv1);
		    }
		    return Fractal==0?frsell1:frsell2;
		}		
		public IContext Context { get; set; }
	}
}
