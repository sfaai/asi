//common stuff used throughout views


function ConvertIdToByteStr(instr) {
    var outstr = "";
    var i;
    var myChar;
    for (i = 0; i < instr.length; i++) {
        myChar = instr.charCodeAt(i);
        outstr = outstr + '0' + myChar;
    }
    return outstr;
}