<%
function ExtractXMLdata(strInputXML)

     Dim strXML, bElegant, nStartPos
     
     strXML = strInputXML

	'If bElegant is true, we will use the more elegant method of extracting
	'only the data node and using that as XML
	
	'If bElegant if false, we will use the cheesy method of extracting the XML
	'as text and using search and replace techniques to customize our data.
	bElegant = false     
     
    if bElegant then
		'Extract node
		'rename node to our names
		'Note: The "Elegant" conversion will be coded in a future version.
    else
     
		'remove everything before out data
		nStartPos = InStr(strXML, "<rs:data>")			        
		strXML = Mid(strXML, nStartPos)
			        
		'Replace the generic node names with the names in our pages
		strXML = Replace(strXML, "rs:data", "root")
		strXML = Replace(strXML, "z:row", "Customers")
			        
		'Replace everything after out data
		strXML = Replace(strXML, "</xml>", "")
			        
	End If
			
	ExtractXMLData = strXML
end function
    
function Strip(strInput)

	'NOTE: The following solution is just temporary.  A better approach
	'might be to replace the foreign characters with their appropriate
	'escape sequence.  This will be done in due time. Patience Grasshopper.

	dim strStripped
	
	strStripped = "" & strInput
	
	strStripped = Replace(strStripped, "á", "a")
	strStripped = Replace(strStripped, "é", "e")
	strStripped = Replace(strStripped, "í", "i")
	strStripped = Replace(strStripped, "ó", "o")
	strStripped = Replace(strStripped, "ú", "u")
	
	strStripped = Replace(strStripped, "ß", "B")
	strStripped = Replace(strStripped, "æ", "B")	



	strStripped = Replace(strStripped, "ê", "e")
	strStripped = Replace(strStripped, "à", "a")
	strStripped = Replace(strStripped, "ã", "a")			

	
	strStripped = Replace(strStripped, "ö", "o")
	strStripped = Replace(strStripped, "ä", "a")
	strStripped = Replace(strStripped, "å", "a")
	strStripped = Replace(strStripped, "è", "e")
	strStripped = Replace(strStripped, "Å", "A")
	strStripped = Replace(strStripped, "ü", "u")		
	strStripped = Replace(strStripped, "ñ", "n")				
	strStripped = replace(strStripped, "&", "AND")
	'strStripped = replace(strStripped, "'", "")
	strStripped = replace(strStripped, "ª", "")
	strStripped = replace(strStripped, "ç", "c")	


	Strip = strStripped
end function
%>

<%  
Sub AddDomElement(objDom, strName, strValue)

    Dim objNode
    
    'Add new node
    Set objNode = objDom.createElement(strName)
    objNode.Text = ""&strValue
    objDom.documentElement.appendChild objNode


End Sub

  
Sub AppendChildNode(objParentNode, strName, strValue)

    Dim objNode
    Dim objDom
    
    
    'Add ClaimInfo nodes
    set objDOM = server.CreateObject("Microsoft.XMLDom")
    Set objNode = objDom.createElement(strName)
    objNode.Text = strValue
    objParentNode.appendChild objNode


End Sub


Function GetNodeValue(oElem, strNodePath)


    Dim oChildNode
    Dim vReturnValue
    

	Set oChildNode = oElem.selectSingleNode(strNodePath)
	If Not (oChildNode Is Nothing) Then
		vReturnValue = oChildNode.nodeTypedValue
	End If

    GetNodeValue = vReturnValue

End Function








Sub AddCPTElement(objDom, strValue, strStatus, strAmount)

    Dim objNode
    Dim objCPTNode
    
    
    'Create CPT nodes
    Set objCPTNode = objDom.createElement("cpt")
    objDom.documentElement.appendChild objCPTNode
    

    'Append Child nodes to the newly created CPT node
    AppendChildNode objCPTNode, "value", strValue
    AppendChildNode objCPTNode, "status", strStatus
    AppendChildNode objCPTNode, "amount", strAmount

End Sub
  


function getCPTelement(objDOM, strNodePath)
	dim oNodeList
	dim arCpt()
	dim arReturn(), i
	
	
	Set oNodeList = objDOM.selectNodes(strNodePath)
	
	'Set oNodeList = objDOM.selectSingleNode("/ClaimInfo/cpt")

	
	strCPT = oChildNode.nodeTypedValue
	strICD9 = GetNodeValue(objDOM, "/ClaimInfo/icd9")
	strCPT = GetNodeValue(objDOM, "/ClaimInfo/cpt/value")
	
	redim arCpt(GC_max_cpt)
	
    For Each oNode In oNodeList
		i = i + 1
        redim arCpt(GC_max_cpt)
        redim preserve arReturn(i)
        arCpt(GC_CPT) = oNode.Code.nodeTypedValue
        arCpt(GC_Modifier) = oNode.modifier.nodeTypedValue
        arCpt(GC_icd9) = oNode.icd9Code.nodeTypedValue
		arCpt(GC_Dos) = oNode.dos.nodeTypedValue
		arCpt(GC_Amount) = oNode.Amount.nodeTypedValue
		arCpt(GC_Upin) = oNode.upin.nodeTypedValue
        arReturn(i-1) = arCpt
    Next
end function



function getOneCPTelement(oNode)
	dim arCpt(), i
	
    redim arCpt(GC_max_cpt)
    
    arCpt(GC_CPT) = GetNodeValue(oNode, "cptCode")

    arCpt(GC_Modifier) = GetNodeValue(oNode, "modifier")
    arCpt(GC_icd9) = replace(GetNodeValue(oNode, "icd9Code"), ".", "")
	arCpt(GC_Dos) = GetNodeValue(oNode, "dos")
	arCpt(GC_Amount) = GetNodeValue(oNode, "amount")
	arCpt(GC_Upin) = GetNodeValue(oNode, "upin")

    getOneCPTelement = arCpt
end function



Sub AddCPTElement2(objClaimDOM)
    dim objCPTNote, objErrNode, i, nMax
    
    'Create CPT nodes
    Set objCPTNode = objClaimDOM.createElement("cpt")
    objClaimDOM.documentElement.appendChild objCPTNode
    

    'Append Child nodes to the newly created CPT node
    AppendChildNode objCPTNode, "amount", arCpt(GC_Amount)
    AppendChildNode objCPTNode, "cptCode", arCpt(GC_CPT)
    AppendChildNode objCPTNode, "icd9Code", arCpt(GC_icd9)
    AppendChildNode objCPTNode, "dos", arCpt(GC_Dos)
    AppendChildNode objCPTNode, "modifier", arCpt(GC_Modifier)
    AppendChildNode objCPTNode, "upin", arCpt(GC_Upin)

	nMax = ubound(arError)
	for i = 0 to nMax -1
		'Set objErrNode = objCPTNode.createElement("cptError")
		'objCPTNode.documentElement.appendChild objErrNode
		'AppendChildNode objErrNode, "cptErrorCode", arError(i)
		'AppendChildNode objErrNode, "cptErrorMsg", arMsg(i)
		AddOutCPTErrorElement objClaimDOM, objCPTNode, arError(i), arMsg(i)
	next
    
End Sub

Sub AddOutCPTErrorElement(objClaimDOM, objParentNode, nErrorNumber, strErrDesc)

    Dim objNode, objErrNode, objDOM 
    
    
    'First Create the Error nodes that you wish to append
    Set objErrNode = objClaimDOM.createElement("CptError")
    objParentNode.appendChild objErrNode
    

    'Append Child nodes to the newly created CPT node
    AppendChildNode objErrNode, "ErrorNumber", nErrorNumber
    AppendChildNode objErrNode, "ErrorDesc", strErrDesc
    
End Sub


%>