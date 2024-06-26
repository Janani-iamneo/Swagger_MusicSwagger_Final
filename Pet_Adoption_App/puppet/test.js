const puppeteer = require('puppeteer');
(async () => {
    const browser = await puppeteer.launch({
      headless: false,
      args: ['--headless', '--disable-gpu', '--remote-debugging-port=9222', '--no-sandbox', '--disable-setuid-sandbox']
    });

    const page = await browser.newPage();
    try{
      // await page.goto('https://api.example.com/');
      await page.goto('https://8081-fcebdccccdbcfacbdcbaeadbebabcdebdca.premiumproject.examly.io/');
      await page.setViewport({
        width:1200,
        height:1200,
      })
        
      const headers = await page.evaluate(() => {
        const thElements = Array.from(document.querySelectorAll('table th'));
        return thElements.map(th => th.textContent.trim());
      });
    // console.log(headers);
      if (headers[0] === 'Party Hall ID' && headers[1] === 'Name' && headers[2] === 'Capacity' && headers[3] ==='Availability'){
        const rowCount = await page.$$eval('table tbody tr', rows => rows.length);
    // console.log(rowCount);
        if (rowCount > 0) {      
          await page.waitForSelector('table tbody tr', { timeout: 5000 } );
          console.log('TESTCASE:In_Index_Page_Table_Headers_and_Rows_are_Correct_and_Exists:success');
        }else{          
          console.log('TESTCASE:In_Index_Page_Table_Headers_and_Rows_are_Correct_and_Exists:failure');
        }
      }else{
        console.log('TESTCASE:In_Index_Page_Table_Headers_and_Rows_are_Correct_and_Exists:failure');
      }
    }
    catch(e){
      console.log('TESTCASE:In_Index_Page_Table_Headers_and_Rows_are_Correct_and_Exists:failure');
    }

    const page2 = await browser.newPage();
    try {
      await page2.goto('https://8081-fcebdccccdbcfacbdcbaeadbebabcdebdca.premiumproject.examly.io/');
      await page2.setViewport({
        width: 1200,
        height: 1200,
      });
      await page2.waitForSelector('#bookButton', { timeout: 2000 });
      await page2.click('#bookButton');
      const urlAfterClick = page2.url();
      await page2.waitForSelector('#customerName', { timeout: 2000 });
      await page2.waitForSelector('#contactNumber', { timeout: 2000 });
      await page2.waitForSelector('#durationInMinutes', { timeout: 2000 });
      const Message = await page2.$eval('h1', element => element.textContent.toLowerCase());
      // console.log("Message",Message);
    if(Message.includes("make a reservation")&&urlAfterClick.toLowerCase().includes('/booking/book'))
    {
    console.log('TESTCASE:Check_Successful_Navigation_to_PartyHall_Reservation_Page_and_Presence_of_Name_Email_Phone_Elements:success');
    }    
    else{
    console.log('TESTCASE:Check_Successful_Navigation_to_PartyHall_Reservation_Page_and_Presence_of_Name_Email_Phone_Elements:failure');
    }
    } catch (e) {
      console.log('TESTCASE:Check_Successful_Navigation_to_PartyHall_Reservation_Page_and_Presence_of_Name_Email_Phone_Elements:failure');
    } 


  const page3 = await browser.newPage();

    try {        
      await page3.goto('https://8081-fcebdccccdbcfacbdcbaeadbebabcdebdca.premiumproject.examly.io/');
      await page3.setViewport({
        width: 1200,
        height: 800,
      });

      await page3.waitForSelector('#delete', { timeout: 2000 });
      await page3.click('#delete');
      const urlAfterClick = page3.url();  
      await page3.waitForSelector('h1', { timeout: 5000 });
      await page3.waitForSelector('#delete', { timeout: 2000 });
      await page3.waitForSelector('#cancel', { timeout: 2000 });
      const Message1 = await page3.$eval('h1', element => element.textContent.toLowerCase());
      // console.log("Message", Message1);
    if(Message1.includes("delete party hall") && urlAfterClick.toLowerCase().includes('/partyhall/delete'))
    {
    console.log('TESTCASE:Check_Successful_Navigation_to_PartyHall_Deleting_Page_and_Presence_of_h2_delete_cancel_Elements:success');
    }    
    else{
    console.log('TESTCASE:Check_Successful_Navigation_to_PartyHall_Deleting_Page_and_Presence_of_h2_delete_cancel_Elements:failure');
    }

    }
  catch(e){
    console.log('TESTCASE:Check_Successful_Navigation_to_PartyHall_Deleting_Page_and_Presence_of_h2_delete_cancel_Elements:failure');
  }
  
  finally{
    await page.close();
    // await page1.close();
    await page2.close();
    await page3.close();
    await browser.close();
  }
  
})();