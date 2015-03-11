using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Runtime.Design;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace com.webkingsoft.JSONSource_110
{
    public partial class JsonSourceUI : Form
    {
        // RTF Instructions
        #region
        private const string rtf = @"{\rtf1\adeflang1025\ansi\ansicpg1252\uc1\adeff31507\deff0\stshfdbch31506\stshfloch31506\stshfhich31506\stshfbi31507\deflang1040\deflangfe1040\themelang1040\themelangfe0\themelangcs0{\fonttbl{\f0\fbidi \froman\fcharset0\fprq2{\*\panose 02020603050405020304}Times New Roman;}{\f2\fbidi \fmodern\fcharset0\fprq1{\*\panose 02070309020205020404}Courier New;}
{\f3\fbidi \froman\fcharset2\fprq2{\*\panose 05050102010706020507}Symbol;}{\f10\fbidi \fnil\fcharset2\fprq2{\*\panose 05000000000000000000}Wingdings;}{\f34\fbidi \froman\fcharset1\fprq2{\*\panose 02040503050406030204}Cambria Math;}
{\f36\fbidi \froman\fcharset0\fprq2{\*\panose 02040503050406030204}Cambria;}{\f37\fbidi \fswiss\fcharset0\fprq2{\*\panose 020f0502020204030204}Calibri;}{\flomajor\f31500\fbidi \froman\fcharset0\fprq2{\*\panose 02020603050405020304}Times New Roman;}
{\fdbmajor\f31501\fbidi \froman\fcharset0\fprq2{\*\panose 02020603050405020304}Times New Roman;}{\fhimajor\f31502\fbidi \froman\fcharset0\fprq2{\*\panose 02040503050406030204}Cambria;}
{\fbimajor\f31503\fbidi \froman\fcharset0\fprq2{\*\panose 02020603050405020304}Times New Roman;}{\flominor\f31504\fbidi \froman\fcharset0\fprq2{\*\panose 02020603050405020304}Times New Roman;}
{\fdbminor\f31505\fbidi \froman\fcharset0\fprq2{\*\panose 02020603050405020304}Times New Roman;}{\fhiminor\f31506\fbidi \fswiss\fcharset0\fprq2{\*\panose 020f0502020204030204}Calibri;}
{\fbiminor\f31507\fbidi \froman\fcharset0\fprq2{\*\panose 02020603050405020304}Times New Roman;}{\f40\fbidi \froman\fcharset238\fprq2 Times New Roman CE;}{\f41\fbidi \froman\fcharset204\fprq2 Times New Roman Cyr;}
{\f43\fbidi \froman\fcharset161\fprq2 Times New Roman Greek;}{\f44\fbidi \froman\fcharset162\fprq2 Times New Roman Tur;}{\f45\fbidi \froman\fcharset177\fprq2 Times New Roman (Hebrew);}{\f46\fbidi \froman\fcharset178\fprq2 Times New Roman (Arabic);}
{\f47\fbidi \froman\fcharset186\fprq2 Times New Roman Baltic;}{\f48\fbidi \froman\fcharset163\fprq2 Times New Roman (Vietnamese);}{\f60\fbidi \fmodern\fcharset238\fprq1 Courier New CE;}{\f61\fbidi \fmodern\fcharset204\fprq1 Courier New Cyr;}
{\f63\fbidi \fmodern\fcharset161\fprq1 Courier New Greek;}{\f64\fbidi \fmodern\fcharset162\fprq1 Courier New Tur;}{\f65\fbidi \fmodern\fcharset177\fprq1 Courier New (Hebrew);}{\f66\fbidi \fmodern\fcharset178\fprq1 Courier New (Arabic);}
{\f67\fbidi \fmodern\fcharset186\fprq1 Courier New Baltic;}{\f68\fbidi \fmodern\fcharset163\fprq1 Courier New (Vietnamese);}{\f400\fbidi \froman\fcharset238\fprq2 Cambria CE;}{\f401\fbidi \froman\fcharset204\fprq2 Cambria Cyr;}
{\f403\fbidi \froman\fcharset161\fprq2 Cambria Greek;}{\f404\fbidi \froman\fcharset162\fprq2 Cambria Tur;}{\f407\fbidi \froman\fcharset186\fprq2 Cambria Baltic;}{\f408\fbidi \froman\fcharset163\fprq2 Cambria (Vietnamese);}
{\f410\fbidi \fswiss\fcharset238\fprq2 Calibri CE;}{\f411\fbidi \fswiss\fcharset204\fprq2 Calibri Cyr;}{\f413\fbidi \fswiss\fcharset161\fprq2 Calibri Greek;}{\f414\fbidi \fswiss\fcharset162\fprq2 Calibri Tur;}
{\f417\fbidi \fswiss\fcharset186\fprq2 Calibri Baltic;}{\f418\fbidi \fswiss\fcharset163\fprq2 Calibri (Vietnamese);}{\flomajor\f31508\fbidi \froman\fcharset238\fprq2 Times New Roman CE;}
{\flomajor\f31509\fbidi \froman\fcharset204\fprq2 Times New Roman Cyr;}{\flomajor\f31511\fbidi \froman\fcharset161\fprq2 Times New Roman Greek;}{\flomajor\f31512\fbidi \froman\fcharset162\fprq2 Times New Roman Tur;}
{\flomajor\f31513\fbidi \froman\fcharset177\fprq2 Times New Roman (Hebrew);}{\flomajor\f31514\fbidi \froman\fcharset178\fprq2 Times New Roman (Arabic);}{\flomajor\f31515\fbidi \froman\fcharset186\fprq2 Times New Roman Baltic;}
{\flomajor\f31516\fbidi \froman\fcharset163\fprq2 Times New Roman (Vietnamese);}{\fdbmajor\f31518\fbidi \froman\fcharset238\fprq2 Times New Roman CE;}{\fdbmajor\f31519\fbidi \froman\fcharset204\fprq2 Times New Roman Cyr;}
{\fdbmajor\f31521\fbidi \froman\fcharset161\fprq2 Times New Roman Greek;}{\fdbmajor\f31522\fbidi \froman\fcharset162\fprq2 Times New Roman Tur;}{\fdbmajor\f31523\fbidi \froman\fcharset177\fprq2 Times New Roman (Hebrew);}
{\fdbmajor\f31524\fbidi \froman\fcharset178\fprq2 Times New Roman (Arabic);}{\fdbmajor\f31525\fbidi \froman\fcharset186\fprq2 Times New Roman Baltic;}{\fdbmajor\f31526\fbidi \froman\fcharset163\fprq2 Times New Roman (Vietnamese);}
{\fhimajor\f31528\fbidi \froman\fcharset238\fprq2 Cambria CE;}{\fhimajor\f31529\fbidi \froman\fcharset204\fprq2 Cambria Cyr;}{\fhimajor\f31531\fbidi \froman\fcharset161\fprq2 Cambria Greek;}{\fhimajor\f31532\fbidi \froman\fcharset162\fprq2 Cambria Tur;}
{\fhimajor\f31535\fbidi \froman\fcharset186\fprq2 Cambria Baltic;}{\fhimajor\f31536\fbidi \froman\fcharset163\fprq2 Cambria (Vietnamese);}{\fbimajor\f31538\fbidi \froman\fcharset238\fprq2 Times New Roman CE;}
{\fbimajor\f31539\fbidi \froman\fcharset204\fprq2 Times New Roman Cyr;}{\fbimajor\f31541\fbidi \froman\fcharset161\fprq2 Times New Roman Greek;}{\fbimajor\f31542\fbidi \froman\fcharset162\fprq2 Times New Roman Tur;}
{\fbimajor\f31543\fbidi \froman\fcharset177\fprq2 Times New Roman (Hebrew);}{\fbimajor\f31544\fbidi \froman\fcharset178\fprq2 Times New Roman (Arabic);}{\fbimajor\f31545\fbidi \froman\fcharset186\fprq2 Times New Roman Baltic;}
{\fbimajor\f31546\fbidi \froman\fcharset163\fprq2 Times New Roman (Vietnamese);}{\flominor\f31548\fbidi \froman\fcharset238\fprq2 Times New Roman CE;}{\flominor\f31549\fbidi \froman\fcharset204\fprq2 Times New Roman Cyr;}
{\flominor\f31551\fbidi \froman\fcharset161\fprq2 Times New Roman Greek;}{\flominor\f31552\fbidi \froman\fcharset162\fprq2 Times New Roman Tur;}{\flominor\f31553\fbidi \froman\fcharset177\fprq2 Times New Roman (Hebrew);}
{\flominor\f31554\fbidi \froman\fcharset178\fprq2 Times New Roman (Arabic);}{\flominor\f31555\fbidi \froman\fcharset186\fprq2 Times New Roman Baltic;}{\flominor\f31556\fbidi \froman\fcharset163\fprq2 Times New Roman (Vietnamese);}
{\fdbminor\f31558\fbidi \froman\fcharset238\fprq2 Times New Roman CE;}{\fdbminor\f31559\fbidi \froman\fcharset204\fprq2 Times New Roman Cyr;}{\fdbminor\f31561\fbidi \froman\fcharset161\fprq2 Times New Roman Greek;}
{\fdbminor\f31562\fbidi \froman\fcharset162\fprq2 Times New Roman Tur;}{\fdbminor\f31563\fbidi \froman\fcharset177\fprq2 Times New Roman (Hebrew);}{\fdbminor\f31564\fbidi \froman\fcharset178\fprq2 Times New Roman (Arabic);}
{\fdbminor\f31565\fbidi \froman\fcharset186\fprq2 Times New Roman Baltic;}{\fdbminor\f31566\fbidi \froman\fcharset163\fprq2 Times New Roman (Vietnamese);}{\fhiminor\f31568\fbidi \fswiss\fcharset238\fprq2 Calibri CE;}
{\fhiminor\f31569\fbidi \fswiss\fcharset204\fprq2 Calibri Cyr;}{\fhiminor\f31571\fbidi \fswiss\fcharset161\fprq2 Calibri Greek;}{\fhiminor\f31572\fbidi \fswiss\fcharset162\fprq2 Calibri Tur;}
{\fhiminor\f31575\fbidi \fswiss\fcharset186\fprq2 Calibri Baltic;}{\fhiminor\f31576\fbidi \fswiss\fcharset163\fprq2 Calibri (Vietnamese);}{\fbiminor\f31578\fbidi \froman\fcharset238\fprq2 Times New Roman CE;}
{\fbiminor\f31579\fbidi \froman\fcharset204\fprq2 Times New Roman Cyr;}{\fbiminor\f31581\fbidi \froman\fcharset161\fprq2 Times New Roman Greek;}{\fbiminor\f31582\fbidi \froman\fcharset162\fprq2 Times New Roman Tur;}
{\fbiminor\f31583\fbidi \froman\fcharset177\fprq2 Times New Roman (Hebrew);}{\fbiminor\f31584\fbidi \froman\fcharset178\fprq2 Times New Roman (Arabic);}{\fbiminor\f31585\fbidi \froman\fcharset186\fprq2 Times New Roman Baltic;}
{\fbiminor\f31586\fbidi \froman\fcharset163\fprq2 Times New Roman (Vietnamese);}}{\colortbl;\red0\green0\blue0;\red0\green0\blue255;\red0\green255\blue255;\red0\green255\blue0;\red255\green0\blue255;\red255\green0\blue0;\red255\green255\blue0;
\red255\green255\blue255;\red0\green0\blue128;\red0\green128\blue128;\red0\green128\blue0;\red128\green0\blue128;\red128\green0\blue0;\red128\green128\blue0;\red128\green128\blue128;\red192\green192\blue192;
\caccentone\ctint255\cshade191\red54\green95\blue145;\caccentone\ctint255\cshade255\red79\green129\blue189;\chyperlink\ctint255\cshade255\red0\green0\blue255;}{\*\defchp \f31506\fs22\lang1040\langfe1033\langfenp1033 }{\*\defpap 
\ql \li0\ri0\sa200\sl276\slmult1\widctlpar\wrapdefault\aspalpha\aspnum\faauto\adjustright\rin0\lin0\itap0 }\noqfpromote {\stylesheet{\ql \li0\ri0\sa200\sl276\slmult1\widctlpar\wrapdefault\aspalpha\aspnum\faauto\adjustright\rin0\lin0\itap0 \rtlch\fcs1 
\af31507\afs22\alang1025 \ltrch\fcs0 \f31506\fs22\lang1040\langfe1033\cgrid\langnp1040\langfenp1033 \snext0 \sqformat \spriority0 \styrsid16273993 Normal;}{\s1\ql \li0\ri0\sb480\sl276\slmult1
\keep\keepn\widctlpar\wrapdefault\aspalpha\aspnum\faauto\outlinelevel0\adjustright\rin0\lin0\itap0 \rtlch\fcs1 \ab\af31503\afs28\alang1025 \ltrch\fcs0 \b\fs28\cf17\lang1040\langfe1033\loch\f31502\hich\af31502\dbch\af31501\cgrid\langnp1040\langfenp1033 
\sbasedon0 \snext0 \slink16 \sqformat \spriority9 \styrsid14113050 heading 1;}{\s2\ql \li0\ri0\sb200\sl276\slmult1\keep\keepn\widctlpar\wrapdefault\aspalpha\aspnum\faauto\outlinelevel1\adjustright\rin0\lin0\itap0 \rtlch\fcs1 \ab\af31503\afs26\alang1025 
\ltrch\fcs0 \b\fs26\cf18\lang1040\langfe1033\loch\f31502\hich\af31502\dbch\af31501\cgrid\langnp1040\langfenp1033 \sbasedon0 \snext0 \slink17 \sunhideused \sqformat \spriority9 \styrsid14113050 heading 2;}{\*\cs10 \additive 
\ssemihidden \sunhideused \spriority1 Default Paragraph Font;}{\*
\ts11\tsrowd\trftsWidthB3\trpaddl108\trpaddr108\trpaddfl3\trpaddft3\trpaddfb3\trpaddfr3\trcbpat1\trcfpat1\tblind0\tblindtype3\tsvertalt\tsbrdrt\tsbrdrl\tsbrdrb\tsbrdrr\tsbrdrdgl\tsbrdrdgr\tsbrdrh\tsbrdrv \ql \li0\ri0\sa200\sl276\slmult1
\widctlpar\wrapdefault\aspalpha\aspnum\faauto\adjustright\rin0\lin0\itap0 \rtlch\fcs1 \af31507\afs22\alang1025 \ltrch\fcs0 \f31506\fs22\lang1040\langfe1033\cgrid\langnp1040\langfenp1033 \snext11 \ssemihidden \sunhideused Normal Table;}{
\s15\ql \li720\ri0\sa200\sl276\slmult1\widctlpar\wrapdefault\aspalpha\aspnum\faauto\adjustright\rin0\lin720\itap0\contextualspace \rtlch\fcs1 \af31507\afs22\alang1025 \ltrch\fcs0 \f31506\fs22\lang1040\langfe1033\cgrid\langnp1040\langfenp1033 
\sbasedon0 \snext15 \sqformat \spriority34 \styrsid16273993 List Paragraph;}{\*\cs16 \additive \rtlch\fcs1 \ab\af31503\afs28 \ltrch\fcs0 \b\fs28\cf17\loch\f31502\hich\af31502\dbch\af31501 \sbasedon10 \slink1 \slocked \spriority9 \styrsid14113050 
Titolo 1 Carattere;}{\*\cs17 \additive \rtlch\fcs1 \ab\af31503\afs26 \ltrch\fcs0 \b\fs26\cf18\loch\f31502\hich\af31502\dbch\af31501 \sbasedon10 \slink2 \slocked \spriority9 \styrsid14113050 Titolo 2 Carattere;}{\*\cs18 \additive \rtlch\fcs1 \af0 
\ltrch\fcs0 \ul\cf19 \sbasedon10 \sunhideused \styrsid9242161 Hyperlink;}}{\*\listtable{\list\listtemplateid-1019591596\listhybrid{\listlevel\levelnfc23\levelnfcn23\leveljc0\leveljcn0\levelfollow0\levelstartat1\levelspace360\levelindent0{\leveltext
\leveltemplateid68157451\'01\u-3880 ?;}{\levelnumbers;}\f10\fbias0 \fi-360\li720\lin720 }{\listlevel\levelnfc23\levelnfcn23\leveljc0\leveljcn0\levelfollow0\levelstartat1\lvltentative\levelspace360\levelindent0{\leveltext\leveltemplateid68157443
\'01o;}{\levelnumbers;}\f2\fbias0 \fi-360\li1440\lin1440 }{\listlevel\levelnfc23\levelnfcn23\leveljc0\leveljcn0\levelfollow0\levelstartat1\lvltentative\levelspace360\levelindent0{\leveltext\leveltemplateid68157445\'01\u-3929 ?;}{\levelnumbers;}
\f10\fbias0 \fi-360\li2160\lin2160 }{\listlevel\levelnfc23\levelnfcn23\leveljc0\leveljcn0\levelfollow0\levelstartat1\lvltentative\levelspace360\levelindent0{\leveltext\leveltemplateid68157441\'01\u-3913 ?;}{\levelnumbers;}\f3\fbias0 
\fi-360\li2880\lin2880 }{\listlevel\levelnfc23\levelnfcn23\leveljc0\leveljcn0\levelfollow0\levelstartat1\lvltentative\levelspace360\levelindent0{\leveltext\leveltemplateid68157443\'01o;}{\levelnumbers;}\f2\fbias0 \fi-360\li3600\lin3600 }{\listlevel
\levelnfc23\levelnfcn23\leveljc0\leveljcn0\levelfollow0\levelstartat1\lvltentative\levelspace360\levelindent0{\leveltext\leveltemplateid68157445\'01\u-3929 ?;}{\levelnumbers;}\f10\fbias0 \fi-360\li4320\lin4320 }{\listlevel\levelnfc23\levelnfcn23\leveljc0
\leveljcn0\levelfollow0\levelstartat1\lvltentative\levelspace360\levelindent0{\leveltext\leveltemplateid68157441\'01\u-3913 ?;}{\levelnumbers;}\f3\fbias0 \fi-360\li5040\lin5040 }{\listlevel\levelnfc23\levelnfcn23\leveljc0\leveljcn0\levelfollow0
\levelstartat1\lvltentative\levelspace360\levelindent0{\leveltext\leveltemplateid68157443\'01o;}{\levelnumbers;}\f2\fbias0 \fi-360\li5760\lin5760 }{\listlevel\levelnfc23\levelnfcn23\leveljc0\leveljcn0\levelfollow0\levelstartat1\lvltentative\levelspace360
\levelindent0{\leveltext\leveltemplateid68157445\'01\u-3929 ?;}{\levelnumbers;}\f10\fbias0 \fi-360\li6480\lin6480 }{\listname ;}\listid835459106}{\list\listtemplateid-1588979062\listhybrid{\listlevel\levelnfc23\levelnfcn23\leveljc0\leveljcn0\levelfollow0
\levelstartat1\levelspace360\levelindent0{\leveltext\leveltemplateid68157441\'01\u-3913 ?;}{\levelnumbers;}\f3\fbias0 \fi-360\li720\lin720 }{\listlevel\levelnfc23\levelnfcn23\leveljc0\leveljcn0\levelfollow0\levelstartat1\lvltentative\levelspace360
\levelindent0{\leveltext\leveltemplateid68157443\'01o;}{\levelnumbers;}\f2\fbias0 \fi-360\li1440\lin1440 }{\listlevel\levelnfc23\levelnfcn23\leveljc0\leveljcn0\levelfollow0\levelstartat1\lvltentative\levelspace360\levelindent0{\leveltext
\leveltemplateid68157445\'01\u-3929 ?;}{\levelnumbers;}\f10\fbias0 \fi-360\li2160\lin2160 }{\listlevel\levelnfc23\levelnfcn23\leveljc0\leveljcn0\levelfollow0\levelstartat1\lvltentative\levelspace360\levelindent0{\leveltext\leveltemplateid68157441
\'01\u-3913 ?;}{\levelnumbers;}\f3\fbias0 \fi-360\li2880\lin2880 }{\listlevel\levelnfc23\levelnfcn23\leveljc0\leveljcn0\levelfollow0\levelstartat1\lvltentative\levelspace360\levelindent0{\leveltext\leveltemplateid68157443\'01o;}{\levelnumbers;}\f2\fbias0 
\fi-360\li3600\lin3600 }{\listlevel\levelnfc23\levelnfcn23\leveljc0\leveljcn0\levelfollow0\levelstartat1\lvltentative\levelspace360\levelindent0{\leveltext\leveltemplateid68157445\'01\u-3929 ?;}{\levelnumbers;}\f10\fbias0 \fi-360\li4320\lin4320 }
{\listlevel\levelnfc23\levelnfcn23\leveljc0\leveljcn0\levelfollow0\levelstartat1\lvltentative\levelspace360\levelindent0{\leveltext\leveltemplateid68157441\'01\u-3913 ?;}{\levelnumbers;}\f3\fbias0 \fi-360\li5040\lin5040 }{\listlevel\levelnfc23\levelnfcn23
\leveljc0\leveljcn0\levelfollow0\levelstartat1\lvltentative\levelspace360\levelindent0{\leveltext\leveltemplateid68157443\'01o;}{\levelnumbers;}\f2\fbias0 \fi-360\li5760\lin5760 }{\listlevel\levelnfc23\levelnfcn23\leveljc0\leveljcn0\levelfollow0
\levelstartat1\lvltentative\levelspace360\levelindent0{\leveltext\leveltemplateid68157445\'01\u-3929 ?;}{\levelnumbers;}\f10\fbias0 \fi-360\li6480\lin6480 }{\listname ;}\listid1040015725}}{\*\listoverridetable{\listoverride\listid1040015725
\listoverridecount0\ls1}{\listoverride\listid835459106\listoverridecount0\ls2}}{\*\pgptbl {\pgp\ipgp0\itap0\li0\ri0\sb0\sa0}}{\*\rsidtbl \rsid355512\rsid3101223\rsid5314640\rsid7953512\rsid9242161\rsid10164376\rsid14113050\rsid16273993}{\mmathPr
\mmathFont34\mbrkBin0\mbrkBinSub0\msmallFrac0\mdispDef1\mlMargin0\mrMargin0\mdefJc1\mwrapIndent1440\mintLim0\mnaryLim1}{\info{\author Alberto Geniola (ICONSULTING)}{\operator Alberto Geniola (ICONSULTING)}{\creatim\yr2014\mo8\dy22\hr14\min14}
{\revtim\yr2014\mo8\dy22\hr17\min24}{\version5}{\edmins190}{\nofpages1}{\nofwords326}{\nofchars1863}{\*\company ICONSULTING s.r.l.}{\nofcharsws2185}{\vern49167}}{\*\xmlnstbl {\xmlns1 http://schemas.microsoft.com/office/word/2003/wordml}}
\paperw11906\paperh16838\margl1134\margr1134\margt1417\margb1134\gutter0\ltrsect 
\deftab708\widowctrl\ftnbj\aenddoc\hyphhotz283\trackmoves0\trackformatting1\donotembedsysfont1\relyonvml0\donotembedlingdata0\grfdocevents0\validatexml1\showplaceholdtext0\ignoremixedcontent0\saveinvalidxml0
\showxmlerrors1\noxlattoyen\expshrtn\noultrlspc\dntblnsbdb\nospaceforul\formshade\horzdoc\dgmargin\dghspace180\dgvspace180\dghorigin1134\dgvorigin1417\dghshow1\dgvshow1
\jexpand\viewkind1\viewscale98\pgbrdrhead\pgbrdrfoot\splytwnine\ftnlytwnine\htmautsp\nolnhtadjtbl\useltbaln\alntblind\lytcalctblwd\lyttblrtgr\lnbrkrule\nobrkwrptbl\snaptogridincell\allowfieldendsel\wrppunct
\asianbrkrule\rsidroot16273993\newtblstyruls\nogrowautofit\usenormstyforlist\noindnmbrts\felnbrelev\nocxsptable\indrlsweleven\noafcnsttbl\afelev\utinl\hwelev\spltpgpar\notcvasp\notbrkcnstfrctbl\notvatxbx\krnprsnet\cachedcolbal \nouicompat \fet0
{\*\wgrffmtfilter 2450}\nofeaturethrottle1\ilfomacatclnup0\ltrpar \sectd \ltrsect\linex0\headery708\footery708\colsx708\endnhere\sectlinegrid360\sectdefaultcl\sftnbj {\*\pnseclvl1\pnucrm\pnstart1\pnindent720\pnhang {\pntxta .}}{\*\pnseclvl2
\pnucltr\pnstart1\pnindent720\pnhang {\pntxta .}}{\*\pnseclvl3\pndec\pnstart1\pnindent720\pnhang {\pntxta .}}{\*\pnseclvl4\pnlcltr\pnstart1\pnindent720\pnhang {\pntxta )}}{\*\pnseclvl5\pndec\pnstart1\pnindent720\pnhang {\pntxtb (}{\pntxta )}}{\*\pnseclvl6
\pnlcltr\pnstart1\pnindent720\pnhang {\pntxtb (}{\pntxta )}}{\*\pnseclvl7\pnlcrm\pnstart1\pnindent720\pnhang {\pntxtb (}{\pntxta )}}{\*\pnseclvl8\pnlcltr\pnstart1\pnindent720\pnhang {\pntxtb (}{\pntxta )}}{\*\pnseclvl9\pnlcrm\pnstart1\pnindent720\pnhang 
{\pntxtb (}{\pntxta )}}\pard\plain \ltrpar\s2\ql \li0\ri0\sb200\sl276\slmult1\keep\keepn\widctlpar\wrapdefault\aspalpha\aspnum\faauto\outlinelevel1\adjustright\rin0\lin0\itap0\pararsid14113050 \rtlch\fcs1 \ab\af31503\afs26\alang1025 \ltrch\fcs0 
\b\fs26\cf18\lang1040\langfe1033\loch\af31502\hich\af31502\dbch\af31501\cgrid\langnp1040\langfenp1033 {\rtlch\fcs1 \af31503 \ltrch\fcs0 \lang16\langfe1033\langnp16\insrsid355512\charrsid14113050 {\*\bkmkstart OLE_LINK1}{\*\bkmkstart OLE_LINK2}
\hich\af31502\dbch\af31501\loch\f31502 Instructions}{\rtlch\fcs1 \af31503 \ltrch\fcs0 \lang16\langfe1033\langnp16\insrsid16273993\charrsid14113050 
\par }\pard\plain \ltrpar\qj \li0\ri0\sa200\sl276\slmult1\widctlpar\wrapdefault\aspalpha\aspnum\faauto\adjustright\rin0\lin0\itap0\pararsid16273993 \rtlch\fcs1 \af31507\afs22\alang1025 \ltrch\fcs0 \f31506\fs22\lang1040\langfe1033\cgrid\langnp1040\langfenp1033 
{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid16273993\charrsid14113050 By default this component expects to receive a }{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 
\b\f37\fs20\lang1033\langfe1033\langnp1033\insrsid16273993\charrsid3101223 JSON Array as input}{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid16273993\charrsid14113050 
. If the input isn't an array but an object, you can provide the}{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid3101223  absolute}{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 
\f37\fs20\lang1033\langfe1033\langnp1033\insrsid16273993\charrsid14113050  path to follow }{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid3101223 down to}{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 
\f37\fs20\lang1033\langfe1033\langnp1033\insrsid16273993\charrsid14113050  }{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid3101223 the target}{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 
\f37\fs20\lang1033\langfe1033\langnp1033\insrsid16273993\charrsid14113050  array, starting from the root. There are two possibilities: }{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid16273993\charrsid14113050 
\par {\listtext\pard\plain\ltrpar \s15 \rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f3\fs20\lang1033\langfe1033\langnp1033\insrsid3101223 \loch\af3\dbch\af31506\hich\f3 \'b7\tab}}\pard\plain \ltrpar\s15\qj \fi-360\li720\ri0\sa200\sl276\slmult1
\widctlpar\wrapdefault\aspalpha\aspnum\faauto\ls1\adjustright\rin0\lin720\itap0\pararsid16273993\contextualspace \rtlch\fcs1 \af31507\afs22\alang1025 \ltrch\fcs0 \f31506\fs22\lang1040\langfe1033\cgrid\langnp1040\langfenp1033 {\rtlch\fcs1 \af37\afs20 
\ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid3101223 XPath-like complex strings}{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid16273993\charrsid14113050 
\par {\listtext\pard\plain\ltrpar \s15 \rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f3\fs20\lang1033\langfe1033\langnp1033\insrsid16273993\charrsid14113050 \loch\af3\dbch\af31506\hich\f3 \'b7\tab}Simple Key}{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 
\f37\fs20\lang1033\langfe1033\langnp1033\insrsid3101223 -Key}{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid16273993\charrsid14113050  concatenation
\par }\pard\plain \ltrpar\qj \li0\ri0\sa200\sl276\slmult1\widctlpar\wrapdefault\aspalpha\aspnum\faauto\adjustright\rin0\lin0\itap0\pararsid16273993 \rtlch\fcs1 \af31507\afs22\alang1025 \ltrch\fcs0 \f31506\fs22\lang1040\langfe1033\cgrid\langnp1040\langfenp1033 
{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid3101223 The first option}{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid16273993\charrsid14113050 
 is only supported if the component is configured as High Memory Consuption, i.e. is available when the entire dataset is parsed and lo}{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid3101223 
aded into memory. If you select}{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid16273993\charrsid14113050  the Low Memory Consuption}{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 
\f37\fs20\lang1033\langfe1033\langnp1033\insrsid3101223  Option}{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid16273993\charrsid14113050  (which doesn\rquote t anything in memory), you can only use}{\rtlch\fcs1 
\af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid355512\charrsid14113050  the simple key concatenation. }{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid16273993\charrsid14113050 
\par }{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \b\f37\fs20\lang1033\langfe1033\langnp1033\insrsid9242161 Given the following strings:}{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \b\f37\fs20\lang1033\langfe1033\langnp1033\insrsid16273993\charrsid14113050 
\par {\listtext\pard\plain\ltrpar \s15 \rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f10\fs20\lang1033\langfe1033\langnp1033\insrsid16273993\charrsid14113050 \loch\af10\dbch\af31506\hich\f10 \'d8\tab}}\pard\plain \ltrpar\s15\qj \fi-360\li720\ri0\sa200\sl276\slmult1
\widctlpar\wrapdefault\aspalpha\aspnum\faauto\ls2\adjustright\rin0\lin720\itap0\pararsid16273993\contextualspace \rtlch\fcs1 \af31507\afs22\alang1025 \ltrch\fcs0 \f31506\fs22\lang1040\langfe1033\cgrid\langnp1040\langfenp1033 {\rtlch\fcs1 \af37\afs20 
\ltrch\fcs0 \b\f37\fs20\lang1033\langfe1033\langnp1033\insrsid16273993\charrsid14113050 Simple Key concat: \tab }{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid5314640\charrsid5314640 
\par }\pard \ltrpar\s15\qj \li720\ri0\sa200\sl276\slmult1\widctlpar\wrapdefault\aspalpha\aspnum\faauto\adjustright\rin0\lin720\itap0\pararsid5314640\contextualspace {\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 
\f37\fs20\lang1033\langfe1033\langnp1033\insrsid9242161\charrsid9242161 Manufacturers}{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid9242161 .Products}{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 
\f37\fs20\lang1033\langfe1033\langnp1033\insrsid16273993\charrsid14113050 
\par {\listtext\pard\plain\ltrpar \s15 \rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f10\fs20\lang1033\langfe1033\langnp1033\insrsid16273993\charrsid14113050 \loch\af10\dbch\af31506\hich\f10 \'d8\tab}}\pard \ltrpar
\s15\qj \fi-360\li720\ri0\widctlpar\wrapdefault\aspalpha\aspnum\faauto\ls2\adjustright\rin0\lin720\itap0\pararsid9242161\contextualspace {\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \b\f37\fs20\lang1033\langfe1033\langnp1033\insrsid16273993\charrsid14113050 
XPath string: \tab \tab }{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \b\f37\fs20\lang1033\langfe1033\langnp1033\insrsid5314640 {\*\bkmkstart OLE_LINK7}{\*\bkmkstart OLE_LINK8}{\*\bkmkstart OLE_LINK9}
\par }\pard \ltrpar\s15\qj \li720\ri0\widctlpar\wrapdefault\aspalpha\aspnum\faauto\adjustright\rin0\lin720\itap0\pararsid5314640\contextualspace {\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid9242161\charrsid9242161 $.
{\*\bkmkstart OLE_LINK4}{\*\bkmkstart OLE_LINK5}{\*\bkmkstart OLE_LINK6}Manufacturers{\*\bkmkend OLE_LINK4}{\*\bkmkend OLE_LINK5}{\*\bkmkend OLE_LINK6}[?(@.Name == 'Acme Co')]}{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 
\f37\fs20\lang1033\langfe1033\langnp1033\insrsid9242161 .Products}{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \b\f37\fs20\lang1033\langfe1033\langnp1033\insrsid16273993\charrsid14113050 {\*\bkmkend OLE_LINK7}{\*\bkmkend OLE_LINK8}{\*\bkmkend OLE_LINK9}
\par }\pard\plain \ltrpar\qj \li0\ri0\widctlpar\wrapdefault\aspalpha\aspnum\faauto\adjustright\rin0\lin0\itap0\pararsid16273993 \rtlch\fcs1 \af31507\afs22\alang1025 \ltrch\fcs0 \f31506\fs22\lang1040\langfe1033\cgrid\langnp1040\langfenp1033 {\rtlch\fcs1 
\af37\afs20 \ltrch\fcs0 \b\f37\fs20\lang1033\langfe1033\langnp1033\insrsid9242161 
\par }{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid9242161\charrsid9242161 Both of them will produce the same results, but in a different way. }{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 
\f37\fs20\lang1033\langfe1033\langnp1033\insrsid9242161 Using the simple key concat, the component will first look for object key \'93Manufacturers\'94. After the key is located, the key \'93Products\'94 is searched inside that sub-object. When the \'93
Products\'94 is found, every sub-object content is parsed using the Input-Output information of this GUI. }{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid9242161\charrsid7953512 Please note that the \'93Products\'94
 key of the \'93Contoso\'94 manufacturer won\rquote t be parsed}{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid9242161 : using the simple key concatenation }{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 
\b\f37\fs20\lang1033\langfe1033\langnp1033\insrsid9242161\charrsid7953512 only the first matching key is walked}{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid9242161 . In other words, there\rquote s no way to }{
\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid7953512 parse an array nested into another array but the first. If you needed to only parse the Products array of Contoso manufacturer, you would use High 
Memory Consuption mode and type the following string: 
\par }\pard \ltrpar\qc \li0\ri0\widctlpar\wrapdefault\aspalpha\aspnum\faauto\adjustright\rin0\lin0\itap0\pararsid7953512 {\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid7953512\charrsid9242161 $.Manufacturers[?(@.Name == }{
\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid7953512 \lquote Contoso}{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid7953512\charrsid9242161 ')]}{\rtlch\fcs1 \af37\afs20 
\ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid7953512 .Products}{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid9242161\charrsid9242161 
\par }\pard \ltrpar\qj \li0\ri0\widctlpar\wrapdefault\aspalpha\aspnum\faauto\adjustright\rin0\lin0\itap0\pararsid16273993 {\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \b\f37\fs20\lang1033\langfe1033\langnp1033\insrsid9242161 
\par }{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \b\f37\fs20\lang1033\langfe1033\langnp1033\insrsid7953512\charrsid14113050 
\par }{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid3101223 {\*\bkmkend OLE_LINK1}{\*\bkmkend OLE_LINK2}As you would imagine, the XPath-like syntax is very powerful. However there are downsides, like the high memory 
consumption (the whole json three must be stored in memory before being walked). So you should chose this operation mode only if the expected dataset is composed by a few rows (depending on the hardware, we suggest to use it under 100k rows datasets). }{
\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid16273993 
\par }{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid9242161 A simple tutorial on XPath-like strings is available here: }{\field{\*\fldinst {\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 
\f37\fs20\lang1033\langfe1033\langnp1033\insrsid9242161  HYPERLINK ""}{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid9242161\charrsid9242161 
http://james.newtonking.com/archive/2014/02/01/json-net-6-0-release-1-%E2%80%93-jsonpath-and-f-support}{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid9242161 "" }}{\fldrslt {\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 
\cs18\f37\fs20\ul\cf19\lang1033\langfe1033\langnp1033\insrsid9242161\charrsid12085704 http://james.newtonking.com/archive/2014/02/01/json-net-6-0-release-1-%E2%80%93-jsonpath-and-f-support}}}\sectd \ltrsect
\linex0\headery708\footery708\colsx708\endnhere\sectlinegrid360\sectdefaultcl\sftnbj {\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 \f37\fs20\lang1033\langfe1033\langnp1033\insrsid9242161  .}{\rtlch\fcs1 \af37\afs20 \ltrch\fcs0 
\f37\fs20\lang1033\langfe1033\langnp1033\insrsid9242161\charrsid9242161 
\par }{\*\themedata 504b030414000600080000002100e9de0fbfff0000001c020000130000005b436f6e74656e745f54797065735d2e786d6cac91cb4ec3301045f748fc83e52d4a
9cb2400825e982c78ec7a27cc0c8992416c9d8b2a755fbf74cd25442a820166c2cd933f79e3be372bd1f07b5c3989ca74aaff2422b24eb1b475da5df374fd9ad
5689811a183c61a50f98f4babebc2837878049899a52a57be670674cb23d8e90721f90a4d2fa3802cb35762680fd800ecd7551dc18eb899138e3c943d7e503b6
b01d583deee5f99824e290b4ba3f364eac4a430883b3c092d4eca8f946c916422ecab927f52ea42b89a1cd59c254f919b0e85e6535d135a8de20f20b8c12c3b0
0c895fcf6720192de6bf3b9e89ecdbd6596cbcdd8eb28e7c365ecc4ec1ff1460f53fe813d3cc7f5b7f020000ffff0300504b030414000600080000002100a5d6
a7e7c0000000360100000b0000005f72656c732f2e72656c73848fcf6ac3300c87ef85bd83d17d51d2c31825762fa590432fa37d00e1287f68221bdb1bebdb4f
c7060abb0884a4eff7a93dfeae8bf9e194e720169aaa06c3e2433fcb68e1763dbf7f82c985a4a725085b787086a37bdbb55fbc50d1a33ccd311ba548b6309512
0f88d94fbc52ae4264d1c910d24a45db3462247fa791715fd71f989e19e0364cd3f51652d73760ae8fa8c9ffb3c330cc9e4fc17faf2ce545046e37944c69e462
a1a82fe353bd90a865aad41ed0b5b8f9d6fd010000ffff0300504b0304140006000800000021006b799616830000008a0000001c0000007468656d652f746865
6d652f7468656d654d616e616765722e786d6c0ccc4d0ac3201040e17da17790d93763bb284562b2cbaebbf600439c1a41c7a0d29fdbd7e5e38337cedf14d59b
4b0d592c9c070d8a65cd2e88b7f07c2ca71ba8da481cc52c6ce1c715e6e97818c9b48d13df49c873517d23d59085adb5dd20d6b52bd521ef2cdd5eb9246a3d8b
4757e8d3f729e245eb2b260a0238fd010000ffff0300504b030414000600080000002100a3eaf6e1a9060000a61b0000160000007468656d652f7468656d652f
7468656d65312e786d6cec594d6f1b4518be23f11f467b6f6327761a4775aad8b11b48d346b15bd4e37877bc3bcdecce6a669cd437d41e9190100571a012370e
08a8d44a5ccaaf09144191fa1778676677bd13af49d24650417368bdb3cffbfd31efcc5ebd763f66e890084979d2f6ea976b1e2289cf039a846deff6b07f69cd
4352e124c08c27a4ed4d89f4ae6dbcffde55bcae22121304f4895cc76d2f522a5d5f5a923e2c637999a7248177632e62ace051844b81c047c037664bcbb5daea
528c69e2a104c7c076083428a0e8d6784c7de26de4ec7b0c64244aea059f8981664e329a123638a86b849cca2e13e810b3b60792027e3424f7958718960a5eb4
bd9af9f39636ae2ee1f58c88a905b425babef9cbe83282e060d9c814e1a8105aef375a57b60afe06c0d43caed7eb757bf5829f0160df074bad2e659e8dfe5abd
93f32c81eccf79dedd5ab3d670f125fe2b733ab73a9d4eb395e962991a90fdd998c3afd5561b9bcb0ede802cbe39876f7436bbdd55076f4016bf3a87ef5f69ad
365cbc01458c260773681dd07e3fe35e40c69c6d57c2d700be56cbe033146443915d5ac498276a51aec5f81e177d006820c38a26484d5332c63ee47117c72341
b11680d7092ebdb14bbe9c5bd2b290f4054d55dbfb30c55013337eaf9e7fffeaf95374fce0d9f1839f8e1f3e3c7ef0a365e4506de3242c53bdfcf6b33f1f7f8c
fe78facdcb475f54e36519ffeb0f9ffcf2f3e7d540289f993a2fbe7cf2dbb3272fbefaf4f7ef1e55c037051e95e1431a13896e9223b4cf6330cc78c5d59c8cc4
f9288611a6658acd249438c15a4a05ff9e8a1cf4cd296659741c3d3ac4f5e01d01eda30a787d72cf5178108989a2159277a2d801ee72ce3a5c547a6147cb2ab9
793849c26ae16252c6ed637c5825bb8b1327bebd490a7d334f4bc7f06e441c35f7184e140e494214d2eff8012115d6dda5d4f1eb2ef505977cacd05d8a3a9856
ba6448474e36cd88b6690c719956d90cf1767cb37b077538abb27a8b1cba48a80acc2a941f12e6b8f13a9e281c57b11ce298951d7e03aba84ac9c154f8655c4f
2a8874481847bd80485945734b80bda5a0ef60e8589561df65d3d8450a450faa78dec09c97915bfca01be138adc20e681295b11fc80348518cf6b8aa82ef72b7
42f433c401270bc37d871227dca77783db3474549a25887e331115b1bc4eb893bf83291b63625a0d3475a757c734f9bbc6cd28746e2be1e21a37b4ca175f3fae
d0fb6d6dd99bb07b55d5ccf68946bd0877b23d77b908e8dbdf9db7f024d9235010f35bd4bbe6fcae397bfff9e6bca89e2fbe25cfba3034683d8bd841db8cddf1
c2a97b4c191ba8292337a419bc25ec3d411f16359d397392e2149646f0535732087070a1c0860609ae3ea22a1a443885a1bdee6926a1cc588712a55cc261d12c
57f2d67818fc953d6a36f521c4760e89d52e0fecf28a5ececf1a051ba355680eb4b9a015cde0acc256ae644cc1b6d71156d74a9d595adda8669aa223ad3059bb
d81ccac1e58569b0587813861a04a3107879154efd5a341c76302381f6bb8d511e1613858b0c918c7040b21869bbe763543741ca7365ce106d874d067d703cc5
6b25692dcdf60da49d254865718d05e2f2e8bd4994f20c9e4509b89d2c4796948b9325e8a8edb59acb4d0ff9386d7b633827c3cf3885a84b3d476216c27593af
844dfb538bd954f92c9aaddc30b708ea70f561fd3e67b0d3075221d51696914d0df32a4b0196684956ffe526b8f5a20ca8e84667d362650d92e15fd302fce886
968cc7c457e5609756b4efec63d64af9441131888223346213b18f21fc3a55c19e804ab8ee301d413fc0dd9cf6b679e536e7ace8ca37620667d7314b239cb55b
5da279255bb86948850ee6a9a41ed856a9bb31eefca69892bf2053ca69fc3f3345ef2770fbb012e808f870392c30d295d2f6b85011872e9446d4ef0b181c4cef
806c81fb5d780d490557d4e67f410ef5ffb6e62c0f53d6708854fb344482c27ea42241c81eb425937da730ab677b9765c9324626a34aeacad4aa3d2287840d75
0f5cd57bbb87224875d34db236607027f3cf7dce2a6814ea21a75c6f4e272bf65e5b03fff4e4638b198c72fbb0196872ff172a16e3c16c57b5f4863cdf7bcb86
e817b331ab915705082b6d05adacec5f5385736eb5b663cd59bcdccc958328ce5b0c8bc54094c21d12d2ffc0fe4785cfecd70ebda10ef93ef456041f2f343348
1bc8ea4b76f040ba41dac5110c4e76d1269366655d9b8d4eda6bf9667dc1936e21f784b3b5666789f7399d5d0c67ae38a7162fd2d999871d5fdbb585ae86c89e
2c51581ae707191318f3a1acfc258b8fee41a0b7e09bc18429699209be53090c33f4c0d40114bf95684837fe020000ffff0300504b0304140006000800000021
000dd1909fb60000001b010000270000007468656d652f7468656d652f5f72656c732f7468656d654d616e616765722e786d6c2e72656c73848f4d0ac2301484
f78277086f6fd3ba109126dd88d0add40384e4350d363f2451eced0dae2c082e8761be9969bb979dc9136332de3168aa1a083ae995719ac16db8ec8e4052164e
89d93b64b060828e6f37ed1567914b284d262452282e3198720e274a939cd08a54f980ae38a38f56e422a3a641c8bbd048f7757da0f19b017cc524bd62107bd5
001996509affb3fd381a89672f1f165dfe514173d9850528a2c6cce0239baa4c04ca5bbabac4df000000ffff0300504b01022d0014000600080000002100e9de
0fbfff0000001c0200001300000000000000000000000000000000005b436f6e74656e745f54797065735d2e786d6c504b01022d0014000600080000002100a5
d6a7e7c0000000360100000b00000000000000000000000000300100005f72656c732f2e72656c73504b01022d00140006000800000021006b79961683000000
8a0000001c00000000000000000000000000190200007468656d652f7468656d652f7468656d654d616e616765722e786d6c504b01022d001400060008000000
2100a3eaf6e1a9060000a61b00001600000000000000000000000000d60200007468656d652f7468656d652f7468656d65312e786d6c504b01022d0014000600
0800000021000dd1909fb60000001b0100002700000000000000000000000000b30900007468656d652f7468656d652f5f72656c732f7468656d654d616e616765722e786d6c2e72656c73504b050600000000050005005d010000ae0a00000000}
{\*\colorschememapping 3c3f786d6c2076657273696f6e3d22312e302220656e636f64696e673d225554462d3822207374616e64616c6f6e653d22796573223f3e0d0a3c613a636c724d
617020786d6c6e733a613d22687474703a2f2f736368656d61732e6f70656e786d6c666f726d6174732e6f72672f64726177696e676d6c2f323030362f6d6169
6e22206267313d226c743122207478313d22646b3122206267323d226c743222207478323d22646b322220616363656e74313d22616363656e74312220616363
656e74323d22616363656e74322220616363656e74333d22616363656e74332220616363656e74343d22616363656e74342220616363656e74353d22616363656e74352220616363656e74363d22616363656e74362220686c696e6b3d22686c696e6b2220666f6c486c696e6b3d22666f6c486c696e6b222f3e}
{\*\latentstyles\lsdstimax267\lsdlockeddef0\lsdsemihiddendef1\lsdunhideuseddef1\lsdqformatdef0\lsdprioritydef99{\lsdlockedexcept \lsdsemihidden0 \lsdunhideused0 \lsdqformat1 \lsdpriority0 \lsdlocked0 Normal;
\lsdsemihidden0 \lsdunhideused0 \lsdqformat1 \lsdpriority9 \lsdlocked0 heading 1;\lsdqformat1 \lsdpriority9 \lsdlocked0 heading 2;\lsdqformat1 \lsdpriority9 \lsdlocked0 heading 3;\lsdqformat1 \lsdpriority9 \lsdlocked0 heading 4;
\lsdqformat1 \lsdpriority9 \lsdlocked0 heading 5;\lsdqformat1 \lsdpriority9 \lsdlocked0 heading 6;\lsdqformat1 \lsdpriority9 \lsdlocked0 heading 7;\lsdqformat1 \lsdpriority9 \lsdlocked0 heading 8;\lsdqformat1 \lsdpriority9 \lsdlocked0 heading 9;
\lsdpriority39 \lsdlocked0 toc 1;\lsdpriority39 \lsdlocked0 toc 2;\lsdpriority39 \lsdlocked0 toc 3;\lsdpriority39 \lsdlocked0 toc 4;\lsdpriority39 \lsdlocked0 toc 5;\lsdpriority39 \lsdlocked0 toc 6;\lsdpriority39 \lsdlocked0 toc 7;
\lsdpriority39 \lsdlocked0 toc 8;\lsdpriority39 \lsdlocked0 toc 9;\lsdqformat1 \lsdpriority35 \lsdlocked0 caption;\lsdsemihidden0 \lsdunhideused0 \lsdqformat1 \lsdpriority10 \lsdlocked0 Title;\lsdpriority1 \lsdlocked0 Default Paragraph Font;
\lsdsemihidden0 \lsdunhideused0 \lsdqformat1 \lsdpriority11 \lsdlocked0 Subtitle;\lsdsemihidden0 \lsdunhideused0 \lsdqformat1 \lsdpriority22 \lsdlocked0 Strong;\lsdsemihidden0 \lsdunhideused0 \lsdqformat1 \lsdpriority20 \lsdlocked0 Emphasis;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority59 \lsdlocked0 Table Grid;\lsdunhideused0 \lsdlocked0 Placeholder Text;\lsdsemihidden0 \lsdunhideused0 \lsdqformat1 \lsdpriority1 \lsdlocked0 No Spacing;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority60 \lsdlocked0 Light Shading;\lsdsemihidden0 \lsdunhideused0 \lsdpriority61 \lsdlocked0 Light List;\lsdsemihidden0 \lsdunhideused0 \lsdpriority62 \lsdlocked0 Light Grid;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority63 \lsdlocked0 Medium Shading 1;\lsdsemihidden0 \lsdunhideused0 \lsdpriority64 \lsdlocked0 Medium Shading 2;\lsdsemihidden0 \lsdunhideused0 \lsdpriority65 \lsdlocked0 Medium List 1;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority66 \lsdlocked0 Medium List 2;\lsdsemihidden0 \lsdunhideused0 \lsdpriority67 \lsdlocked0 Medium Grid 1;\lsdsemihidden0 \lsdunhideused0 \lsdpriority68 \lsdlocked0 Medium Grid 2;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority69 \lsdlocked0 Medium Grid 3;\lsdsemihidden0 \lsdunhideused0 \lsdpriority70 \lsdlocked0 Dark List;\lsdsemihidden0 \lsdunhideused0 \lsdpriority71 \lsdlocked0 Colorful Shading;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority72 \lsdlocked0 Colorful List;\lsdsemihidden0 \lsdunhideused0 \lsdpriority73 \lsdlocked0 Colorful Grid;\lsdsemihidden0 \lsdunhideused0 \lsdpriority60 \lsdlocked0 Light Shading Accent 1;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority61 \lsdlocked0 Light List Accent 1;\lsdsemihidden0 \lsdunhideused0 \lsdpriority62 \lsdlocked0 Light Grid Accent 1;\lsdsemihidden0 \lsdunhideused0 \lsdpriority63 \lsdlocked0 Medium Shading 1 Accent 1;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority64 \lsdlocked0 Medium Shading 2 Accent 1;\lsdsemihidden0 \lsdunhideused0 \lsdpriority65 \lsdlocked0 Medium List 1 Accent 1;\lsdunhideused0 \lsdlocked0 Revision;
\lsdsemihidden0 \lsdunhideused0 \lsdqformat1 \lsdpriority34 \lsdlocked0 List Paragraph;\lsdsemihidden0 \lsdunhideused0 \lsdqformat1 \lsdpriority29 \lsdlocked0 Quote;\lsdsemihidden0 \lsdunhideused0 \lsdqformat1 \lsdpriority30 \lsdlocked0 Intense Quote;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority66 \lsdlocked0 Medium List 2 Accent 1;\lsdsemihidden0 \lsdunhideused0 \lsdpriority67 \lsdlocked0 Medium Grid 1 Accent 1;\lsdsemihidden0 \lsdunhideused0 \lsdpriority68 \lsdlocked0 Medium Grid 2 Accent 1;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority69 \lsdlocked0 Medium Grid 3 Accent 1;\lsdsemihidden0 \lsdunhideused0 \lsdpriority70 \lsdlocked0 Dark List Accent 1;\lsdsemihidden0 \lsdunhideused0 \lsdpriority71 \lsdlocked0 Colorful Shading Accent 1;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority72 \lsdlocked0 Colorful List Accent 1;\lsdsemihidden0 \lsdunhideused0 \lsdpriority73 \lsdlocked0 Colorful Grid Accent 1;\lsdsemihidden0 \lsdunhideused0 \lsdpriority60 \lsdlocked0 Light Shading Accent 2;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority61 \lsdlocked0 Light List Accent 2;\lsdsemihidden0 \lsdunhideused0 \lsdpriority62 \lsdlocked0 Light Grid Accent 2;\lsdsemihidden0 \lsdunhideused0 \lsdpriority63 \lsdlocked0 Medium Shading 1 Accent 2;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority64 \lsdlocked0 Medium Shading 2 Accent 2;\lsdsemihidden0 \lsdunhideused0 \lsdpriority65 \lsdlocked0 Medium List 1 Accent 2;\lsdsemihidden0 \lsdunhideused0 \lsdpriority66 \lsdlocked0 Medium List 2 Accent 2;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority67 \lsdlocked0 Medium Grid 1 Accent 2;\lsdsemihidden0 \lsdunhideused0 \lsdpriority68 \lsdlocked0 Medium Grid 2 Accent 2;\lsdsemihidden0 \lsdunhideused0 \lsdpriority69 \lsdlocked0 Medium Grid 3 Accent 2;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority70 \lsdlocked0 Dark List Accent 2;\lsdsemihidden0 \lsdunhideused0 \lsdpriority71 \lsdlocked0 Colorful Shading Accent 2;\lsdsemihidden0 \lsdunhideused0 \lsdpriority72 \lsdlocked0 Colorful List Accent 2;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority73 \lsdlocked0 Colorful Grid Accent 2;\lsdsemihidden0 \lsdunhideused0 \lsdpriority60 \lsdlocked0 Light Shading Accent 3;\lsdsemihidden0 \lsdunhideused0 \lsdpriority61 \lsdlocked0 Light List Accent 3;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority62 \lsdlocked0 Light Grid Accent 3;\lsdsemihidden0 \lsdunhideused0 \lsdpriority63 \lsdlocked0 Medium Shading 1 Accent 3;\lsdsemihidden0 \lsdunhideused0 \lsdpriority64 \lsdlocked0 Medium Shading 2 Accent 3;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority65 \lsdlocked0 Medium List 1 Accent 3;\lsdsemihidden0 \lsdunhideused0 \lsdpriority66 \lsdlocked0 Medium List 2 Accent 3;\lsdsemihidden0 \lsdunhideused0 \lsdpriority67 \lsdlocked0 Medium Grid 1 Accent 3;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority68 \lsdlocked0 Medium Grid 2 Accent 3;\lsdsemihidden0 \lsdunhideused0 \lsdpriority69 \lsdlocked0 Medium Grid 3 Accent 3;\lsdsemihidden0 \lsdunhideused0 \lsdpriority70 \lsdlocked0 Dark List Accent 3;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority71 \lsdlocked0 Colorful Shading Accent 3;\lsdsemihidden0 \lsdunhideused0 \lsdpriority72 \lsdlocked0 Colorful List Accent 3;\lsdsemihidden0 \lsdunhideused0 \lsdpriority73 \lsdlocked0 Colorful Grid Accent 3;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority60 \lsdlocked0 Light Shading Accent 4;\lsdsemihidden0 \lsdunhideused0 \lsdpriority61 \lsdlocked0 Light List Accent 4;\lsdsemihidden0 \lsdunhideused0 \lsdpriority62 \lsdlocked0 Light Grid Accent 4;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority63 \lsdlocked0 Medium Shading 1 Accent 4;\lsdsemihidden0 \lsdunhideused0 \lsdpriority64 \lsdlocked0 Medium Shading 2 Accent 4;\lsdsemihidden0 \lsdunhideused0 \lsdpriority65 \lsdlocked0 Medium List 1 Accent 4;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority66 \lsdlocked0 Medium List 2 Accent 4;\lsdsemihidden0 \lsdunhideused0 \lsdpriority67 \lsdlocked0 Medium Grid 1 Accent 4;\lsdsemihidden0 \lsdunhideused0 \lsdpriority68 \lsdlocked0 Medium Grid 2 Accent 4;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority69 \lsdlocked0 Medium Grid 3 Accent 4;\lsdsemihidden0 \lsdunhideused0 \lsdpriority70 \lsdlocked0 Dark List Accent 4;\lsdsemihidden0 \lsdunhideused0 \lsdpriority71 \lsdlocked0 Colorful Shading Accent 4;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority72 \lsdlocked0 Colorful List Accent 4;\lsdsemihidden0 \lsdunhideused0 \lsdpriority73 \lsdlocked0 Colorful Grid Accent 4;\lsdsemihidden0 \lsdunhideused0 \lsdpriority60 \lsdlocked0 Light Shading Accent 5;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority61 \lsdlocked0 Light List Accent 5;\lsdsemihidden0 \lsdunhideused0 \lsdpriority62 \lsdlocked0 Light Grid Accent 5;\lsdsemihidden0 \lsdunhideused0 \lsdpriority63 \lsdlocked0 Medium Shading 1 Accent 5;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority64 \lsdlocked0 Medium Shading 2 Accent 5;\lsdsemihidden0 \lsdunhideused0 \lsdpriority65 \lsdlocked0 Medium List 1 Accent 5;\lsdsemihidden0 \lsdunhideused0 \lsdpriority66 \lsdlocked0 Medium List 2 Accent 5;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority67 \lsdlocked0 Medium Grid 1 Accent 5;\lsdsemihidden0 \lsdunhideused0 \lsdpriority68 \lsdlocked0 Medium Grid 2 Accent 5;\lsdsemihidden0 \lsdunhideused0 \lsdpriority69 \lsdlocked0 Medium Grid 3 Accent 5;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority70 \lsdlocked0 Dark List Accent 5;\lsdsemihidden0 \lsdunhideused0 \lsdpriority71 \lsdlocked0 Colorful Shading Accent 5;\lsdsemihidden0 \lsdunhideused0 \lsdpriority72 \lsdlocked0 Colorful List Accent 5;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority73 \lsdlocked0 Colorful Grid Accent 5;\lsdsemihidden0 \lsdunhideused0 \lsdpriority60 \lsdlocked0 Light Shading Accent 6;\lsdsemihidden0 \lsdunhideused0 \lsdpriority61 \lsdlocked0 Light List Accent 6;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority62 \lsdlocked0 Light Grid Accent 6;\lsdsemihidden0 \lsdunhideused0 \lsdpriority63 \lsdlocked0 Medium Shading 1 Accent 6;\lsdsemihidden0 \lsdunhideused0 \lsdpriority64 \lsdlocked0 Medium Shading 2 Accent 6;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority65 \lsdlocked0 Medium List 1 Accent 6;\lsdsemihidden0 \lsdunhideused0 \lsdpriority66 \lsdlocked0 Medium List 2 Accent 6;\lsdsemihidden0 \lsdunhideused0 \lsdpriority67 \lsdlocked0 Medium Grid 1 Accent 6;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority68 \lsdlocked0 Medium Grid 2 Accent 6;\lsdsemihidden0 \lsdunhideused0 \lsdpriority69 \lsdlocked0 Medium Grid 3 Accent 6;\lsdsemihidden0 \lsdunhideused0 \lsdpriority70 \lsdlocked0 Dark List Accent 6;
\lsdsemihidden0 \lsdunhideused0 \lsdpriority71 \lsdlocked0 Colorful Shading Accent 6;\lsdsemihidden0 \lsdunhideused0 \lsdpriority72 \lsdlocked0 Colorful List Accent 6;\lsdsemihidden0 \lsdunhideused0 \lsdpriority73 \lsdlocked0 Colorful Grid Accent 6;
\lsdsemihidden0 \lsdunhideused0 \lsdqformat1 \lsdpriority19 \lsdlocked0 Subtle Emphasis;\lsdsemihidden0 \lsdunhideused0 \lsdqformat1 \lsdpriority21 \lsdlocked0 Intense Emphasis;
\lsdsemihidden0 \lsdunhideused0 \lsdqformat1 \lsdpriority31 \lsdlocked0 Subtle Reference;\lsdsemihidden0 \lsdunhideused0 \lsdqformat1 \lsdpriority32 \lsdlocked0 Intense Reference;
\lsdsemihidden0 \lsdunhideused0 \lsdqformat1 \lsdpriority33 \lsdlocked0 Book Title;\lsdpriority37 \lsdlocked0 Bibliography;\lsdqformat1 \lsdpriority39 \lsdlocked0 TOC Heading;}}{\*\datastore 010500000200000018000000
4d73786d6c322e534158584d4c5265616465722e362e3000000000000000000000060000
d0cf11e0a1b11ae1000000000000000000000000000000003e000300feff090006000000000000000000000001000000010000000000000000100000feffffff00000000feffffff0000000000000000ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff
ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff
ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff
ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff
fffffffffffffffffdfffffffeffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff
ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff
ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff
ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff
ffffffffffffffffffffffffffffffff52006f006f007400200045006e00740072007900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000016000500ffffffffffffffffffffffff0c6ad98892f1d411a65f0040963251e5000000000000000000000000400a
7b1f1dbecf01feffffff00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000ffffffffffffffffffffffff00000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000ffffffffffffffffffffffff0000000000000000000000000000000000000000000000000000
000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000ffffffffffffffffffffffff000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000105000000000000}}";
#endregion //RTF
        private Variables _vars;
        private Model _model;
        private IServiceProvider _sp;
        private System.Windows.Forms.IWin32Window _parent;
        public JsonSourceUI(System.Windows.Forms.IWin32Window parent, Variables vars, Model model,IServiceProvider sp)
        {
            // Salva i riferimenti in locale
            _parent = parent;
            _vars = vars;
            _model = model;
            _sp = sp;

            // Inizializza la UI
            InitializeComponent();
            uiAdvancedInstructions.Rtf = rtf;

            // Imposta i vari Enumerativi previsti come tipi di dato.
            (uiIOGrid.Columns["OutColumnType"] as DataGridViewComboBoxColumn).DataSource = Enum.GetNames(typeof(DataType));
            uiSourceType.DataSource = Enum.GetNames(typeof(SourceType));

            // Registra l'handler per il settaggio dei valori di default
            uiIOGrid.DefaultValuesNeeded += uiIOGrid_DefaultValuesNeeded;

            // Carico il model
            LoadModel(_model);
        }

        private void uiIOGrid_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["OutColumnType"].Value = Enum.GetName(typeof(DataType), DataType.DT_WSTR);
        }

        /*
         * Carico il modello all'interno della view
         */
        private void LoadModel(Model m)
        {
            // Tipo di sorgente
            uiSourceType.SelectedItem = Enum.GetName(typeof(SourceType),m.SourceType);

            // File Path Custom
            if (m.FilePath == null)
                uiFilePathCustom.Text = "";
            else
                uiFilePathCustom.Text = m.FilePath;

            // File Path variable
            if (m.FilePathVar == null)
                uiFilePathVariable.Text = "";
            else
                uiFilePathVariable.Text = m.FilePathVar;

            //  Web Url custom
            if (m.WebUrl == null)
                uiWebURLCustom.Text = "";
            else
                uiWebURLCustom.Text = m.WebUrl;

            if (m.WebUrlVariable == null)
                uiURLVariable.Text = "";
            else
                uiURLVariable.Text = m.WebUrlVariable;

            // Configura la UI in modo opportuno
            uiVariableFilePathGroup.Enabled = m.SourceType == SourceType.filePathVariable;
            uiFilePathGroup.Enabled = m.SourceType == SourceType.filePath;
            uiCustomUrlGroup.Enabled = m.SourceType == SourceType.WebUrlPath;
            uiVariableUrlGroup.Enabled = m.SourceType == SourceType.WebUrlVariable;

            // Advanced Tab
            if (m.JsonObjectRelativePath == null)
                uiPathToArray.Text = "";
            else
                uiPathToArray.Text = m.JsonObjectRelativePath;
            if (m.CustomLocalTempDir == null)
                uiTempDir.Text = "";
            else
                uiTempDir.Text = m.CustomLocalTempDir;

            uiMemoryModeHigh.Checked = m.OpMode == OperationMode.StoreInMemory;
            uiMemoryModeLow.Checked = m.OpMode == OperationMode.SyncIO;
            

            // Tab IO
            if (m.IoMap != null)
                LoadIO(m.IoMap);
            else
                uiIOGrid.Rows.Clear();
        }

        /*
         * Carica la configurazione di IO nel datagrid della view.
         */
        private void LoadIO(IEnumerable<IOMapEntry> ios)
        {
            uiIOGrid.Rows.Clear();
            foreach(IOMapEntry e in ios)
            {
                int index = uiIOGrid.Rows.Add();
                uiIOGrid.Rows[index].Cells[0].Value = e.InputFieldName;
                uiIOGrid.Rows[index].Cells[1].Value = e.InputFieldLen;
                uiIOGrid.Rows[index].Cells[2].Value = e.OutputColName;
                uiIOGrid.Rows[index].Cells[3].Value = Enum.GetName(typeof(DataType),e.OutputColumnType);
            }
        }


        private void ok_Click(object sender, EventArgs e)
        {
            try {
                // Salva tutti i dettagli nel model
                // setta il model nelle properties del componente.
                // Evito la validazione dalla view, verrà fatta dal component direttamente.
                // - Salva le informazioni riguardanti la sorgente dei dati
                _model.SourceType = (SourceType)Enum.Parse(typeof(SourceType), uiSourceType.SelectedItem.ToString());
                _model.FilePath = uiSourceType.Text == Enum.GetName(typeof(SourceType), SourceType.filePath) ? uiFilePathCustom.Text : null;
                _model.WebUrl = uiSourceType.Text == Enum.GetName(typeof(SourceType), SourceType.WebUrlPath) ? uiWebURLCustom.Text : null;
                if (uiSourceType.Text == Enum.GetName(typeof(SourceType), SourceType.filePathVariable))
                    _model.FilePathVar = uiFilePathVariable.Text;
                else
                    _model.FilePathVar = null;
                if (uiSourceType.Text == Enum.GetName(typeof(SourceType), SourceType.WebUrlVariable))
                    _model.WebUrlVariable = uiURLVariable.Text;
                else
                    _model.WebUrlVariable = null;

                _model.ClearMapping();

                // - Salva le impostazioni di IO
                if (uiIOGrid.IsCurrentCellDirty || uiIOGrid.IsCurrentRowDirty)
                {
                    uiIOGrid.CurrentRow.DataGridView.EndEdit();
                    uiIOGrid.EndEdit();
                    CurrencyManager cm = (CurrencyManager)uiIOGrid.BindingContext[uiIOGrid.DataSource, uiIOGrid.DataMember];
                    cm.EndCurrentEdit();
                }

                int row = 1;
                foreach (DataGridViewRow r in uiIOGrid.Rows)
                {
                    if (r.IsNewRow)
                        continue;
                    string inputName = null;
                    try {
                        inputName = (string)r.Cells[0].Value;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("JSON Field Name on row "+row);
                        return;
                    }
                    int maxLen = -1;
                    try
                    {
                        maxLen = int.Parse((string)r.Cells[1].Value.ToString());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Maximum length is invalid on row " + row);
                        return;
                    }

                    string outName = null;
                    try
                    {
                        outName = (string)r.Cells[2].Value;
                        if (string.IsNullOrEmpty(outName))
                            throw new ArgumentException();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Output Column name is invalid on row " + row);
                        return;
                    }

                    DataType dataType = 0;
                    try
                    {
                        dataType = (DataType)Enum.Parse(typeof(DataType), (string)r.Cells[3].Value);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Column typee is invalid on row " + row);
                        return;
                    }

                    IOMapEntry map = new IOMapEntry();
                    map.InputFieldName = inputName;
                    map.InputFieldLen = maxLen;
                    map.OutputColName = outName;
                    map.OutputColumnType = dataType;

                    _model.AddMapping(map);
                    row++;
                }

                // - Salava le impostazioni avanzate
                if (!string.IsNullOrEmpty(uiTempDir.Text))
                    _model.CustomLocalTempDir = uiTempDir.Text;
                else
                    _model.CustomLocalTempDir = null;
                if (!string.IsNullOrEmpty(uiPathToArray.Text))
                    _model.JsonObjectRelativePath = uiPathToArray.Text;
                else
                    _model.JsonObjectRelativePath = null;

                if (uiMemoryModeHigh.Checked)
                    _model.OpMode = OperationMode.StoreInMemory;
                else if (uiMemoryModeLow.Checked)
                    _model.OpMode = OperationMode.SyncIO;

                DialogResult = DialogResult.OK;

                Close();

            }
            catch (FormatException ex)
            {
                MessageBox.Show("Invalid number (max length) specified. Please fix it.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occurred: " + ex.Message);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void openFileButton_Click(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SourceType newVal = (SourceType)Enum.Parse(typeof(SourceType), (sender as ComboBox).SelectedItem.ToString());
            // Aggiorna la UI in modo opportuno
            uiFilePathGroup.Enabled = newVal == SourceType.filePath;
            uiVariableFilePathGroup.Enabled = newVal == SourceType.filePathVariable;
            uiCustomUrlGroup.Enabled = newVal == SourceType.WebUrlPath;
            uiVariableUrlGroup.Enabled = newVal == SourceType.WebUrlVariable;

        }

        private void FilePathFromVariable_Click(object sender, EventArgs e)
        {
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Json Text Files (*.json)|*.json|Text Files (*.txt)|*.txt";
            openFileDialog.FilterIndex = 1;
            DialogResult rd = openFileDialog.ShowDialog();
            if (rd == System.Windows.Forms.DialogResult.OK)
            {
                uiFilePathCustom.Text = openFileDialog.FileName;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        /**
         * Controlla che il website sia raggiungibile.
         */
        private void uiTestWebURL_Click(object sender, EventArgs e)
        {
            try
            {
                HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(uiWebURLCustom.Text);
                rq.Credentials = CredentialCache.DefaultCredentials;
                HttpWebResponse resp = (HttpWebResponse)rq.GetResponse();
                if (resp.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show("Warning: status code received was " + resp.StatusCode+", i.e. "+resp.StatusDescription);
                }
                // Se l'utente vuole avere una previsione del file che scaricherà, lo scarichiamo
                DialogResult dr = MessageBox.Show("Do you want to download and preview the JSON data?","Preview",MessageBoxButtons.OKCancel,MessageBoxIcon.Question);
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    string tmp = Path.GetTempFileName()+".json";
                    StreamWriter sw = new StreamWriter(new FileStream(tmp,FileMode.OpenOrCreate));
                    StreamReader sr = new StreamReader(resp.GetResponseStream());
                    char[] buff = new char[4096];
                    int read = 0;
                    while ((read = sr.ReadBlock(buff, 0, buff.Length)) > 0)
                        sw.Write(buff, 0, read);
                    sw.Close();
                    Process p = new Process();
                    p.StartInfo.FileName = "notepad";
                    p.StartInfo.Arguments = tmp;
                    p.Start();
                }
                resp.Close();
            }
            catch (UriFormatException ex)
            {
                MessageBox.Show("Error: invalid URL format. " + ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show("Error: URL cannot be null. " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }




        private void uiSelectURLVariable_Click(object sender, EventArgs e)
        {
            VariableChooser vc = new VariableChooser(_vars);
            DialogResult dr = vc.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Microsoft.SqlServer.Dts.Runtime.Variable v = vc.GetResult();
                uiURLVariable.Text = v.QualifiedName;
            }
        }

        private void uiBrowseFilePathVariable_Click(object sender, EventArgs e)
        {
            VariableChooser vc = new VariableChooser(_vars);
            DialogResult dr = vc.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Microsoft.SqlServer.Dts.Runtime.Variable v = vc.GetResult();
                uiFilePathVariable.Text = v.QualifiedName;
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IDtsVariableService vservice = (IDtsVariableService)_sp.GetService(typeof(IDtsVariableService));
            Microsoft.SqlServer.Dts.Runtime.Variable vv = vservice.PromptAndCreateVariable(_parent, null, null, "User", typeof(string));
            if (vv != null)
                uiWebURLCustom.Text = vv.QualifiedName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IDtsVariableService vservice = (IDtsVariableService)_sp.GetService(typeof(IDtsVariableService));
            Microsoft.SqlServer.Dts.Runtime.Variable vv = vservice.PromptAndCreateVariable(_parent, null,null,"User",typeof(string));
            if (vv != null)
                uiFilePathVariable.Text = vv.QualifiedName;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
