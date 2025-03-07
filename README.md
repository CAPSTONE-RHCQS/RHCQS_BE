<div align="center">
  <h1 align="center">Residential Housing Construction Quotation System Back end</h1>
  <h3>Phần mềm quản lý báo giá thi công nhà ở dân dụng</h3>
  <h3>Team: RHCQS - 5 members</h3>
</div>

# Back-end Team

- [**Bui Hieu Khang - BA, Tester**](https://github.com/Bhieukhang)  
  Responsible for business analysis and testing features, ensuring quality and completeness.
  Feature:
  - Manage material
  - Manage labor
  - Manage supplier
  - Dashboard
    

- [**Nguyen Van Tien - BE Main**](https://github.com/nvtiendev)  
  **Feature:**  
  - Final quotation  
  - Package construction  
  - Build calculate construction work  
  - Auth system  
  - Manage profile actor  
  - Manage blog  
  - Notification mobile  

  **Others:**  
  - Deploy project in Azure  

- [**Tran Thi Kim Ngan - BE Main**](https://github.com/ngandolh)  
  **Feature:**  
  - Initial quotation  
  - Processing house design drawing  
  - Contract design, contract construction.  
  - Build calculate construction work  
  - Manage promotion  
  - Chat  
  - Manage template design house  
  - Manage construction  
  - Manage project  

  **Others:**  
  - Cloudinary  
  - Generate PDF  
  - Deploy project in Render, Azure  


<img src="https://res.cloudinary.com/de7pulfdj/image/upload/v1737274025/tyq109gtzgh8xiqlhv4m.png" alt="Swagger RHCQS"/>

# Technology
- NET 8.0
- SQL Server
- Docker
- Third party: Cloudinary, Firebase, Mail
- Server: Azure, Render

# Functional Requirement

## **Module for Customer**

- Information page introducing implemented home construction projects, sample designs, news, blog sharing experiences, ...
- The function allows customers to view standard quotes for construction packages (rough, finished, complete) and current promotional programs.
- The function allows customers to calculate the estimated preliminary construction price according to selected parameters (project type, construction package, sample design, area, number of floors, ...)
- The function allows customers to request a detailed quote for their residential housing construction project.
- The function allows customers to receive and respond (agree, request editing, refuse) quotes from the construction company.
- Customers can discuss with staff about quotes through the chat function in the system.
- Function that allows customers to evaluate and provide feedback on quotation request.
- Manage customer profile and quotation history.

## **Module for Construction Company**

- Function to declare standard price list of construction packages and applicable promotional programs.
- The system allows declaring standard parameters to calculate the estimated preliminary construction price.
- Function to receive and prepare quotes for detailed quote requests for customers' residential housing construction projects.
- The function allows managers to assign employees to make quotations, approve/reject quotations before sending to customers.
- Detailed management of the quotation includes main information: customer information, expected construction project, construction items, implementation time, payment periods, prices, promotions apply, payment period, expected costs (materials, labor, other), expected profit, valid quote period, staff in charge.
- Manage the process of making customer quotes from receiving requests, assigning staff to perform, completing quotes, sending to managers for approval, managers approving/rejecting, sending to customers, receiving customer feedback, finalizing quotes to sign the contract.
- Manage customer feedback of quotation request.
- CRUD implemented projects, sample designs, news, blogs to share experiences.
- Company statistics dashboard on quotes, ...
- Account Management
- Manage categories in the system
