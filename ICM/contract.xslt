<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" encoding="utf-8" indent="yes" />
	<xsl:template match="/">
		<html>
			<head>
				<title>Contract</title>
				<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
				<link rel="stylesheet" href="Styles/Contract.css" type="text/css" media="screen" />
			</head>
			<body>
					<xsl:apply-templates/>
			</body>
		</html>
	</xsl:template>
	
	
	<xsl:template match="contract">
		<div id="contract">
			<div id="header">
				<h1>International Contract Management</h1>
			</div>
			<div id="content">
				<table>
					<tr>
						<td class="title">Titre: </td>
						<td><xsl:value-of select="@title"/></td>
					</tr>
					<tr>
						<td class="title">Type: </td>
						<td><xsl:value-of select="@contractType"/></td>
					</tr>
					<tr>
						<td class="title">date début: </td>
						<td><xsl:value-of select="@startDate"/></td>
					</tr>
					<tr>
						<td class="title">date fin: </td>
						<td><xsl:value-of select="@endDate"/></td>
					</tr>
					<tr>
						<td class="title">Contacts: </td>
						<td>
							<div id="contacts">
								<xsl:apply-templates select="contacts" mode="contactsMode" />
							</div>
						</td>
					</tr>
					<tr>
						<td class="title">Destinations:</td>
						<td>
							<div id="destinations">
								<xsl:apply-templates select="destinations" mode="destinationsMode" />
							</div>
						</td>
					</tr>
				</table>
			</div>
			
			<div id="footer">
				<p>HES-SO Fribourg - Vincent Ischi, Baptiste Wicht, Kean Mariotti</p>
			</div>
		</div>
	</xsl:template>
	
	<!-- Contacts -->
	<xsl:template match="person" mode="contactsMode">
				<xsl:value-of select="@role"/> : <xsl:value-of select="@name"/>&#160;<xsl:value-of select="@firstName"/> (<xsl:value-of select="@phone"/>) 
			<br/>
	</xsl:template>
	
	<!-- Destinations -->
	<xsl:template match="destination" mode="destinationsMode">
				<xsl:variable name= "id"><xsl:value-of select= "@id"/></xsl:variable>
				<xsl:value-of select="//contract/departments/department[@id=$id]/@name"/>, 
				<xsl:value-of select="//contract/departments/department[@id=$id]/@institutionName"/>, 
				<xsl:value-of select="//contract/departments/department[@id=$id]/@institutionCity"/> (<xsl:value-of select="//contract/departments/department[@id=$id]/@institutionCountry"/>)
			<br/>
	</xsl:template>
</xsl:stylesheet>