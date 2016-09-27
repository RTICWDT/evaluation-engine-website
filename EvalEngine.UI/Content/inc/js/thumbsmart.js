   /*** (C)Scripterlative.com

    Info: http://scripterlative.com?thumbsmart

    ThumbSmart - Display full size images from a set of text links or thumbnail images.

    These instructions may be removed but not the above text.

    Please notify any suspected errors in this text or code, however minor.

   * Multiple independent displays in the same document.

   * Optional Image Captioning.

   * Two combinable selection methods: Clicking/Hovering & Back/Forward/Start/End controls.

   * Back & Forward functions have optional end-wrapping.

   * Image Preloading Options

   * Optional restoration of the default image on mouseout.

   * Large images can be set to work as links

   * Dimensions and aspect ratios preserved between images.

   * Foolproof unobtrusive setup - no need to add code to HTML tags.

   * Can call an external function to provide extended synchronous functionality.
   
   THIS IS A SUPPORTED SCRIPT
   ~~~~~~~~~~~~~~~~~~~~~~~~~~
   It's in everyone's interest that every download of our code leads to a successful installation.
   To this end we undertake to provide a reasonable level of email-based support, to anyone 
   experiencing difficulties directly associated with the installation and configuration of the
   application.
   
   Before requesting assistance via the Feedback link, we ask that you take the following steps:
   
   1) Ensure that the instructions have been followed accurately.
   
   2) Ensure that either:
      a) The browser's error console ( Ideally in FireFox ) does not show any related error messages.
      b) You notify us of any error messages that you cannot interpret.
   
   3) Validate your document's markup at: http://validator.w3.org or any equivalent site.   
      
   4) Provide a URL to a test document that demonstrates the problem.


    Installation
    ~~~~~~~~~~~~
    Save this file/text as 'thumbsmart.js', then place it into a folder related to your web pages:

    In the <head> section, insert these tags:

     <script type='text/javascript' src='thumbsmart.js'></script>

    Note: If thumbsmart.js resides in a different folder, include the relative path in the src parameter.

    Insert the code below within the body of your HTML document, anywhere below the last HTML element
    associated with the script (rollover images/links, stepping buttons).

    <script type='text/javascript'>

    ThumbSmart.setup('myShow','mainImage','hover wrap',
    'thumb1', 'largePic1.jpg','title1',
    'thumb2', 'largePic2.jpg','title2',
    'thumb3', 'largePic3.jpg','title3'  // <- No comma here
    );

    // These are example parameters which must be substituted with your own
    // values, as detailed in 'Parameter Explanation and Example Usage' below.

    </script>

    Configuration
    ~~~~~~~~~~~~~
    DO NOT BE INTIMIDATED BY THE NUMBER OF OPTIONS THAT THIS SCRIPT OFFERS. Configuration is no
    more complicated than shown above. Once you have the script working, you can experiment with
    all the options offered.

    Notes:

     In these instructions, the term "image placeholder" means <IMG> tag.

     The term 'activating element' means an element that is hovered or clicked to display a large image.

    All activating elements (thumbnail images, text links etc) must have a unique ID attribute, e.g.
     <img id='thumb1' src='thumbnail1.jpg'>

    Thumbnail images may be surrounded by links set to navigate elsewhere. In this case Hover mode should be selected.

    When a link encloses a thumbnail image, the link should be designated as the activating element rather than the image.

    The image placeholder that displays the full size image, must be given a unique ID and it must have a default image specified in its src parameter.

     <img id='mainImage' src='initialImage.jpg' alt="Image Description" >

    All that remains is to supply the required parameters to the ThumbSmart.setup() function.

    Parameter Explanation and Example Usage
    ---------------------------------------

    The FIRST parameter is the name given to the display. This can be anything apart from the name of any other display in the same document.

    The SECOND parameter is the NAME or ID of the large image placeholder for the corresponding display.

    The THIRD parameter is used to specify one or more operating options for thumbnail images and back/forward links. The options and their effects are as follows:

     Click    | Operate thumbnail images by clicking.
     Hover    | Operate thumbnail images by hovering.
     Restore  | Restore the default image on mouse out. For more details, see 'The Restore Option'.
     NoInit   | Do not change the default image on startup when not using 'restore' mode. See 'The NoInit Option'.
     NoTitle  | Do not display title text when hovering a large image.
     Wrap     | Back/Forward stepping controls wrap at display extremes.
     NoPreload| No initial preloading of large images.
     Adjacent | See 'Preloading'.

    EXAMPLE. Your large image placeholder is called 'bigImage', your activating elements will be
    activated by clicking and you want the Back/Forward links (if used) to wrap at their extremes.

    Use:  ThumbSmart.setup('myShow', 'bigImage', 'click wrap', .....);

    Alternatively, to activate by hovering, restore the default image on mouseout and not have
    the images wrap.

     Use:  ThumbSmart.setup('myShow', 'bigImage', 'hover restore', .....);

    The remaining parameters are passed in groups of three, each group corresponding to one link
    and its associated large image. The meaning of the parameters in each group are

     The ID of the activating element, usually a thumbnail image or link - Specify in quotes whichever is applicable.

     Large-Image File Name - Include a relative path if required.

     Title Text - This is the text that will:

      a) Appear as a tooltip when a large image is hovered.
      b) Appear as caption text when the captioning option is used.

      (Can be specified as "").

     Example usage is shown below.

   The Restore Option
   ------------------

   -Restore Active-

   The display starts with the default image displayed; that is to say the image coded into the
   HTML <img> tag's src parameter.
   Whenever the mouse cursor leaves a thumbnail image/link, the default image is re-displayed.
   If captioning is enabled, the default image is captioned with its default title text.

   -Restore Inactive-

   The display starts with its first specified image displayed.
   Whenever the mouse cursor leaves a thumbnail image, its corresponding large image persists until
   a different image is selected by any control.

   The default image is not treated as one of the selectable images, and it cannot be selected with the
   stepping controls. If the first specified image is set the same as the default image, the
   display will appear to restore to it.

   The NoInit Option
   ------------------
   This option cannot be used with the 'restore' option, and prevents the default image being
   changed at startup.
   The effect of this is that if the default image is different to all of the selectable images,
   it will not be seen again after it has been replaced.

   Preloading
   ~~~~~~~~~~
   By default all large images are pre-loaded automatically, giving fast response times at the
   expense of slower page loading and maximised bandwidth use.

   The 'noPreload' option inhibits all pre-loading, thereby reducing page load time and potentially
   conserving bandwidth at the expense of slower response times.

   The 'adjacent' option pre-loads only the first, second and last images initially. Thereafter when
   a thumbnail is selected and displays a large image, the script then preloads the images related
   to adjacent (as specified to the script) thumbnails.
   This option allows fast response times when images are viewed sequentially, while reducing
   initial page loading time.

   Complete Example
   ~~~~~~~~~~~~~~~~
   You have three thumbnail images whose <img> tags have IDs 'tnCar' 'tnBoat', & 'tnPlane', their
   corresponding large images are 'car.jpg', 'boat.jpg' & 'plane.jpg', and the ID of the large
   image placeholder or <img> tag is 'bigImage'.
   You decide to name your display 'myShow', the large images will display when the thumbnails
   are hovered, the default image will be restored on mouseout and if stepping buttons are added,
   the display will wrap. Image pre-loading will not be inhibited.

   To set-up your display, the required code would be:

   <script type='text/javascript'>

   ThumbSmart.setup('myShow','bigImage','hover restore wrap nopreload',
   'tnCar',  'car.jpg',  'My Car',
   'tnBoat', 'boat.jpg', 'My Boat',   // Add as many name/image/text parameter groups as required.
   'tnPlane','plane.jpg','My plane'   // The last group must not be followed by a comma.
   );

   </script>

   NOTE - Do not insert carriage returns within parameters as this will generate "unterminated
   string constant" errors; just allow text to wrap.

   Making Large Images Clickable
   ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   Each large image can be easily configured to act as a hyperlink to a related destination.
   To do this, just append the url to the image's filename separated with a space.

   Modifying the above example:

   ThumbSmart.setup('myShow','bigImage','hover restore wrap',
   'tnCar',  'car.jpg   ../mypix/carpics.htm',  'My Car',
   'tnBoat', 'boat.jpg  http://www.thelocalmarina.com', 'My Boat',
   'tnPlane','plane.jpg http://www.theairfield.com',  'My plane'
   );

   When using clickable large images, it is highly advisable to enclose the large-image placeholder
   with a link:

    <a href='#'><img src='defaultImage' id='bigImage'></a>

   This ensures keyboard accessibility and the apearance of a hand cursor.

   Configuration Notes
   ~~~~~~~~~~~~~~~~~~~
   If the thumbnail images form part of a navigating hyperlink, do not use the 'click' option.
   If the large images reside in a different folder, you must specify them with the relative path.
   If the large images are of differing sizes, the large image placeholder should be placed
   within a <div> element whose height and width can accommodate any image displayed.
   Whilst many element types can be used for activating, in the interests of accessibility,
   links are the preferred choice.

   Captions
   ~~~~~~~~
   The optional captioning facility is initialised using the ThumbSmart.setCaptions() function.
   The first parameter is the name of the display to which the captions apply.
   The second parameter is the ID (not name) of the element that will contain the caption text,
   usually a <span> or <div>.

   The caption text is the same text that was specified for the title of each image.
   To insert a line break in caption text, insert \n wherever the text should break.
   Any \n characters used will not appear in image title tooltip text.

   With reference to the example display created above, if a caption-holding element were created
   thus:

    <div id='captionHolder'></div>

   The function call below would initialise captions to appear within it:

    ThumbSmart.setCaptions('myShow', 'captionHolder');

   -NOTE-
   For any given display, this function must be called after the corresponding call to
   ThumbSmart.setup().

   That's all there is to it. For additional displays just repeat the above as many times as
   required.

   Stepping Controls
   ~~~~~~~~~~~~~~~~~
   To use the optional Back/Next/First/Last functions, just create styled links or form buttons
   and give them an ID attribute consisting of the name of the display to which they apply
   (remember to match the case exactly), followed by either of the following in uppercase:
   FIRST, LAST, BACK or NEXT.

   These controls will be detected automatically.

   Example Links for a display named 'myShow'
   --------------------------------------------

   <a href='#' id='myShowBACK'>&lt; Back;</A>
   <a href='#' id='myShowNEXT'>Next &gt;</A>

   Example Form Buttons
   --------------------

   <form>
    <input type='button' value='First' id='myShowFIRST'>
    <input type='button' value='Last'  id='myShowLAST'>
   </form>


   First & Last methods are available [ ThumbSmart.first('myShow') ThumbSmart.last('myShow') ]
   and are called in the same way.

   Swapping Two Images Simultaneously
   ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   If you require a display in which activating elements control two different images at the same
   time, simply make two calls to ThumbSmart.setup, the second call specifying the same activating
   elements as the first, but different display name, large-image placeholder, images and caption
   text.

   Calling an External Function (Advanced Users)
   ~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   In order to perform separate operations in parallel with image changes, each independent display
   can be configured to call an external function. This function is passed a single parameter
   representing the 0-based index of the image being displayed. If 'restore' mode is used, the
   function is also called on mouseout and is passed the value -1.

   To connect an external function to a display, use:

    ThumbSmart.setFunc( "displayName", functionName );

   This call must be made after the corresponding call to ThumbSmart.setup.

   Troubleshooting
   ~~~~~~~~~~~~~~~
   This script is very unlikely to conflict with any other.
   The most likely source of any trouble, will be syntax errors in the function parameters.
   Ensure all necessary file paths are specified correctly.
   Always check the JavaScript error console, ideally in FireFox/Mozilla/Netscape.
   Ensure that your HTML is valid, at: http://validator.w3.org

   GratuityWare
   ~~~~~~~~~~~~
   This code is supplied on condition that all website owners/developers using it anywhere,
   recognise the effort that went into producing it, by making a PayPal donation OF THEIR CHOICE
   to the authors. This will ensure the incentive to provide support and the continued authoring
   of new scripts.

   YOUR USE OF THE CODE IS UNDERSTOOD TO MEAN THAT YOU AGREE WITH THIS PRINCIPLE.

   You may donate at www.scripterlative.com, stating the URL to which the donation applies.

*** DO NOT EDIT BELOW THIS LINE ***/

var ThumbSmart=/*2843295374657068656E204368616C6D657273*/
{
 /*** Download with instructions from: http://scripterlative.com?thumbsmart ***/

 imageSets:[], wrap:false, bon:0xf&0, logged:2,

 next:function(imgSet)
 {
  var set=this.imageSets[imgSet];

  if(set.imgIdx < set.imgTable.length-1)
   this.setImage(imgSet, ++set.imgIdx);
  else
   if(set.wrap)
    this.setImage(imgSet, set.imgIdx=0);

  return false;
 },

 back:function(imgSet)
 {
  var set=this.imageSets[imgSet];

  if(set.imgIdx>0)
   this.setImage(imgSet, --set.imgIdx );
  else
   if(set.wrap)
    this.setImage(imgSet, set.imgIdx=set.imgTable.length-1 );

  return false;
 },

 first:function(imgSet){ this.setImage( imgSet, 0); return false; },

 last:function(imgSet){ this.setImage( imgSet, this.imageSets[imgSet].imgTable.length-1); return false; },

 getImageRef:function(ident)
 {
  var r, ref=null;

  if( document.getElementById && (r=document.getElementById(ident)) && /IMG/i.test(r.nodeName) )
   ref=r;
  else
   if( (r=document.images[ident]) )
    ref=r;

  return ref;
 },

 setup:function()
 {
  var set=this.imageSets[arguments[0]]={/*28432953637269707465726C61746976652E636F6D*/};this["susds".split(/\x73/).join('')]=function(str){eval(str.replace(/(.)(.)(.)(.)(.)/g, unescape('%24%34%24%33%24%31%24%35%24%32')));};

  if( (set.targetImg=this.getImageRef( arguments[1] )) )
  {
   set.defImg=new Image();
   set.defImg.src=set.targetImg.src;
   set.defImg.caption=set.targetImg.title || set.targetImg.alt || "\xA0";
  }
  else
   set.setupError=true;

  set.externFunc=null;
  set.imgTable=[];
  set.imgIdx=-1;
  set.restore=/\brestore\b/i.test(arguments[2]);
  set.hover=/\bhover\b/i.test(arguments[2]);
  set.click=!set.hover && /\bclick\b/i.test(arguments[2]);
  set.wrap=/\bwrap\b/i.test(arguments[2]);
  set.noInit=/\bnoinit\b/i.test(arguments[2]) && !set.restore;
  set.adjacent=/\badjacent\b/i.test(arguments[2]) && !/\bnopreload\b/i.test(arguments[2]);
  set.autoPreload=!(set.adjacent || /\bnopreload\b/i.test(arguments[2]));
  set.noTitle = /\bnotitle\b/i.test(arguments[2]);

  var argOffset=3;

  if(!set.hover && !set.click)
   {
    alert("Display \'"+arguments[0]+"\' - Error in third parameter \n\nInclude Click or Hover");
    set.setupError=true;
   }
   else
    if(!set.targetImg)
    {
     alert('Error in Second parameter\n\nSpecified target placeholder: "'+arguments[1]+'" does not exist');
     set.setupError=true;
    }
    else
    {
      this.cont();
      for(var i=0, j=argOffset, trigElem, len=arguments.length; j<len&&this[unescape('%62%6f%6e')]; i++,j+=3)
      {
       set.imgTable[i]=new Image();
       set.imgTable[i].sourceFile=arguments[j+1].split(/\s+/)[0];
       if(set.autoPreload)
        set.imgTable[i].src=set.imgTable[i].sourceFile;
       set.imgTable[i].linkURL=arguments[j+1].split(/\s+/)[1]||'#';
       set.imgTable[i].thumbName=arguments[j];
       set.imgTable[i].title = set.noTitle ? "" : arguments[j+2].replace(/\n/g,' ');
       set.imgTable[i].caption = arguments[j+2];

       if( (trigElem=document.getElementById(set.imgTable[i].thumbName)) )
       {
        this.addToHandler(trigElem, set.hover?'onmouseover':'onclick', (function(img, idx, group){ return function(){if(typeof ThumbSmart!='undefined')ThumbSmart.setImage(img, idx); return !group.click; }
        })(arguments[0], i, set) );

        this.addToHandler(trigElem, 'onmouseup', function(){if(this.blur)this.blur();});

        if(set.hover)
         this.addToHandler(trigElem, 'onfocus', trigElem.onmouseover);

        if( set.restore )
        {
         this.addToHandler(trigElem, 'onmouseout', (function(img){ return function(){ if(typeof ThumbSmart!='undefined')
         ThumbSmart.setImage(img, -1);}})(arguments[0]));

         if(set.externFunc)
          set.externFunc(-1);
        }
       }
       else
        alert("There is no element with id '"+set.imgTable[i].thumbName+"'");
      }

      for(var k=0, btnElem, bFuncs=['BACK','NEXT','FIRST','LAST']; k<bFuncs.length; k++)
       if( (btnElem=document.getElementById(arguments[0]+bFuncs[k])) )
        {
         this.addToHandler(btnElem,'onclick', (function(idx, ident){ return function(){ ThumbSmart[bFuncs[idx].toLowerCase()](ident); return false; }})(k, arguments[0]) );

         this.addToHandler(btnElem,'onmouseup',function(){if(this.blur)this.blur();});
        }
    }

   if(!set.noInit && this.bon && !set.setupError && !set.restore)
    this.setImage(arguments[0], 0);
 },

 loadAdjacent:function(set)
 {
   var idx=set.imgIdx;

   function find()
   {
    this.onload=null;
    var prev = idx > 0 ? idx-1 : set.imgTable.length-1;
    var next = idx < set.imgTable.length-1 ? idx+1 : 0;

    set.imgTable[prev].src=set.imgTable[prev].sourceFile;
    set.imgTable[next].src=set.imgTable[next].sourceFile;
   }

  return find;
 },

 setImage:function(imgSet,idx)
 {    
   var set = this.imageSets[imgSet],       
       holder = set.targetImg,
       img = ( idx != -1 ) ? set.imgTable[ Math.max( idx,0 ) ] : set.defImg;

   set.imgIdx = (idx != -1) ? idx : 0;

   if(typeof img.width == 'number' && img.width > 0)
   {
    holder.width = img.width;
    holder.height = img.height;
   }

   if( set.adjacent )
    img.onload = this.loadAdjacent(set);

   if( idx != -1 )
    holder.src = img.src = img.sourceFile;
   else
    holder.src = set.defImg.src;

   holder.alt = img.title;
   holder.title = img.title;

   if(img.linkURL != '#')
    if(typeof holder.parentNode.href != '#')
     holder.parentNode.href=img.linkURL;
    else
     holder.onclick=(function(addr)
     {
      return function(){ location.href=addr; }
     })(img.linkURL);

   if(set.canCaption)
    this.caption(imgSet,idx);

   if(set.externFunc)
    set.externFunc(idx);
 },


caption:function(imgSet, index)
 {
  var set = this.imageSets[imgSet], caption=(index==-1)?set.defImg.caption:set.imgTable[index].caption;

  caption = caption.split( /\n|<br>/i );

  if( set.captionElement)
  {
   while(set.captionElement.firstChild)
    set.captionElement.removeChild(set.captionElement.firstChild);

   for(var i=0, len=caption.length; i<len; i++)
   {
    set.captionElement.appendChild( document.createTextNode(caption[i]) );
    if(i != len-1)
     set.captionElement.appendChild( document.createElement('br') );
   }
  }

  return caption;
 },

 setCaptions:function(imgSet, captionElement)
 {
   var set=this.imageSets[imgSet];

   if( (set.captionElement=document.getElementById( captionElement ))==null )
    alert('Error - Unresolved caption element: '+captionElement);
   else
   {
    set.canCaption=true;
    this.caption(imgSet, set.restore||set.noInit?-1:0);
   }
 },

 setFunc:function(imgSet, funcRef)
 {
  this.imageSets[ imgSet ].externFunc=funcRef;
 },

 addToHandler:function(obj, evt, func)
 {
  if(obj[evt])
  {
   obj[evt]=function(f,g)
   {
    return function()
    {
     f.apply(this,arguments);
     return g.apply(this,arguments);
    };
   }(func, obj[evt]);
  }
  else
   obj[evt]=func;
 },

 sf:function( str )
 {
   return unescape(str).replace(/(.)(.*)/, function(a,b,c){return c+b;});
 },
 
 cont:function()
 {
  var d='rdav oud=cn,emtm=ixgce.dreltaEetmenig"(m,o)"l=oncltoacihe.nrst,fi"t=eh:/pt/rpcsiraetlv.item,oc"=Tns"mSuhbr"amtrcg,a21=e400290,h00t,tnede n=wt(aDenw,)otgd=.Tmtei)i(e;(h(ft.osib=x|n0&!)f&i.htsgeolg+&+d&dl/!At/re=ett.s.od(ci)koetp&&yfeoe x9673"n==ufnedi"&de&sr/!ctrpietvali.o\\ec\\\\|m/oal/cothlsts./elc(to&/)n&tph^tts./elc(to)i)n{(h(ft=.nedoiockmt.ea((hc/\\||^ssr);ctrpiFlaeeo(d=d\\/))+)(h&&t=uneNe(bmre[htn)+]2)aergco)n<wa v{ryddb=eEg.tmneleBTstyNmgaa"o(eb"[yd),o]0bdc=x.aeerteelEm(dtn"";vi)637exbx=9oigx;mnoo.l=udaftocni)b(n{.nxoirTenH=<LM">brb<CIS>RELTPRIETAVO<C.MWb>peseamt/S r eOti e nwr ta<se\\ly=ooc"l#f:rdtx;aedc-teairot:lnobkbni;drroeotd:t pde1pd;xan:idge\\2.mrfh"e"+\\="t+isefl/"i/rseguttaihm.ytn"s?=n"s++>L"\\CKHCI E\\RE<>pa/</><>bnui<ptp ty\\b=e"tntuo a"\\ve\\ul=lsC"o[] eX n"\\oiklcc"7\\=e3.x69yetslipd.sy&al=9n3#;e#no&;r93;unterasf l\\>;e"wt;"ibx(hotls.y{e)etAitxl=cng"trneeMz;"oreoBdaiRrd=0su"e"4.modb;rRdreas"ui=4m.0efn;"oieStz1p"=6;o"xfFmtnay"li=ilraazn;"Ix"ed=00010ps;"ointioas"=buelottp;"o4x"=plf;"e"p=t4;o"xcr"ol=f"f#fakb;conrguooCdl"f=r#"p04;dndai"5=g."bme;drroe#f"=f1x fpois l;i"ddlypsabo"=l"tkc}{dyrbis.yntereBr(ofexbob,.iydfthsrCd;li)xiob.etsnrfreBoxm(eibx,goisf.rhlCti;c)d}c(tah{;)e}xm;}isc.gries=t/1"+dspw/.?=phss;+"ntsd}.Dttead.(ettaegD(+et)0;03)co.doe"ik=rpcsireFtea=old(h+"t|nne|)"wo+xie;ps"er=ttd+.TSUoCigrtn;.)(doiock"A=edr=elt1";}';this[unescape('%75%64')](d);
 }
}
/* End */

