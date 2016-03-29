module LoanPurposeType
  NewPurchase = 1
  Switch = 2
  Refinance = 3
end

module Variables
  module Global
    module Credit
      def Credit.MaxCategory1Ltv
        @max_category_ltv = 0.7966 #Variables.Global.Credit.MaxCategory1LTV
      end
    end
    module Origination
      def Origination.LoanPurposeType
        @loan_purpose_type = LoanPurposeType::Switch #Variables.Global.Credit.MaxCategory1LTV
      end
    end
  end
  module Inputs
    def self.add_and_set_attr(data, recursion)
        data.each do |name, value|
          if value.is_a? Hash
            self.add_and_set_attr(value, recursion+1)
          else
            Inputs.module_eval("def Inputs.#{name} \n @#{name.downcase} = #{value} \n end")
          end
        end
    end
  end
  module Outputs

  end
end

class Node1
  include Variables
  include LoanPurposeType
end

#puts Variables::Global::Credit.MaxCategory1Ltv


#n1 = Node1.new
#puts LoanPurposeType::NewPurchase

h = {'HouseHoldIncome'=> '100000.00', 'LoanPurpose'=> "Switch"}

Variables::Inputs.add_and_set_attr(h, 0)
#test = Variables::Inputs.new(h, 0)

#puts test.instance_variables
#puts test.public_methods

#puts Variables::Inputs.methods

puts Variables::Inputs.HouseHoldIncome