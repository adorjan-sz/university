function yy = HFP8(x,y,xx)
%UNTITLED Summary of this function goes here
%   Detailed explanation goes here
n = length(x);
C = zeros(n+2,n+2);
%first row
C(1,1) =x(1)*6;
C(1,2) = 1;
%midle rows
for i=1:n
    if i<3
    C(i+1,1)= x(i)^3;
    C(i+1,2)= x(i)^2;
    C(i+1,3)= x(i);
    C(i+1,4)= 1;
    else
    for j=1:i+2
        if(j==1)
            C(i+1,1)=x(i)^3;
        elseif(j==2)
            C(i+1,2)=x(i)^2;
        elseif(j==3)
            C(i+1,3)=x(i);
        elseif(j==4)
            C(i+1,4)=1;
        elseif(j>4)
            C(i+1,j)=(x(i)-x(j-3))^3;
        end
    end
    end
end
%last row
for j=1:n+2
        if(j==1)
            C(n+2,1)=x(n)*6;
        elseif(j==2)
            C(n+2,2)=1;
       
        elseif(j>4)
            C(n+2,j)=6*(x(n)-x(j-3));
        end
end
y = [0,y,0];

AllCoeffs = C\(y');
BasePoly = AllCoeffs(1:4);
Betas = AllCoeffs(5:end);

m = length(xx);
OutPut = zeros(1,m);

basei = x(2:end-1);
OutPut = polyval(BasePoly,xx);

for i=1:m
    ssi = max(0,(xx(i)-basei).^3);
    OutPut(i) =OutPut(i) + dot(ssi,Betas);
    % OutPut(i)= polyval(BasePoly,xx(i));
    % j = 2;
    % while(j <= n-1 && xx(i)>=x(j))
    % 
    %     OutPut(i) = OutPut(i) + Betas(j-1)*((xx(i)-x(j))^3);
    % 
    %     j = j+1;
    % end

end
yy = OutPut;
end